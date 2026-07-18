using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using VenEl.DynamicAgents.Core.Interfaces;

namespace VenEl.DynamicAgents.Infrastructure.Clients;

public class GeminiLlmClient : ILlmClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeminiLlmClient(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey), "Gemini API key is required.");
    }

    public async Task<string> GenerateTextAsync(string model, string systemPrompt, string userPrompt, IEnumerable<ITool>? tools = null)
    {
        var geminiModel = model.StartsWith("gemini") ? model : "gemini-2.5-flash"; 
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{geminiModel}:generateContent?key={_apiKey}";

        var toolList = tools?.ToList() ?? new List<ITool>();

        var history = new List<object>
        {
            new
            {
                role = "user",
                parts = new[] { new { text = userPrompt } }
            }
        };

        while (true)
        {
            var payload = new Dictionary<string, object>
            {
                { "system_instruction", new { parts = new[] { new { text = systemPrompt } } } },
                { "contents", history }
            };

            if (toolList.Any())
            {
                var functionDeclarations = toolList.Select(t => new
                {
                    name = t.Name,
                    description = t.Description,
                    parameters = t.ParametersSchema
                });
                
                payload["tools"] = new[]
                {
                    new { function_declarations = functionDeclarations }
                };
            }

            var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Gemini API Error: {response.StatusCode} - {responseString}");
            }

            using var doc = JsonDocument.Parse(responseString);
            var root = doc.RootElement;
            
            if (root.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
            {
                var firstCandidate = candidates[0];
                if (firstCandidate.TryGetProperty("content", out var contentElement) &&
                    contentElement.TryGetProperty("parts", out var parts) && parts.GetArrayLength() > 0)
                {
                    var part = parts[0];
                    
                    if (part.TryGetProperty("functionCall", out var functionCall))
                    {
                        var functionName = functionCall.GetProperty("name").GetString();
                        var argsStr = functionCall.TryGetProperty("args", out var args) ? args.GetRawText() : "{}";
                        
                        var toolToCall = toolList.FirstOrDefault(t => t.Name == functionName);
                        if (toolToCall != null)
                        {
                            history.Add(new
                            {
                                role = "model",
                                parts = new[] { new { functionCall = new { name = functionName, args = JsonNode.Parse(argsStr) } } }
                            });

                            var resultJson = await toolToCall.ExecuteAsync(argsStr);
                            
                            JsonNode resultNode;
                            try { resultNode = JsonNode.Parse(resultJson) ?? JsonNode.Parse("{}"); }
                            catch { resultNode = JsonNode.Parse($"{{\"message\": \"{resultJson.Replace("\"", "\\\"")}\"}}"); }

                            history.Add(new
                            {
                                role = "user",
                                parts = new[]
                                {
                                    new 
                                    { 
                                        functionResponse = new 
                                        { 
                                            name = functionName, 
                                            response = new { name = functionName, content = resultNode } 
                                        } 
                                    }
                                }
                            });
                            
                            continue;
                        }
                    }
                    else if (part.TryGetProperty("text", out var textNode))
                    {
                        return textNode.GetString() ?? string.Empty;
                    }
                }
            }

            return string.Empty;
        }
    }
}

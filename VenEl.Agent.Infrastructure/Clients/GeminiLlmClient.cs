using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VenEl.Agent.Core.Interfaces;

namespace VenEl.Agent.Infrastructure.Clients;

public class GeminiLlmClient : ILlmClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeminiLlmClient(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey), "Gemini API key is required.");
    }

    public async Task<string> GenerateTextAsync(string model, string systemPrompt, string userPrompt)
    {
        var geminiModel = model.StartsWith("gemini") ? model : "gemini-2.5-flash"; 

        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{geminiModel}:generateContent?key={_apiKey}";

        var payload = new
        {
            system_instruction = new
            {
                parts = new[] { new { text = systemPrompt } }
            },
            contents = new[]
            {
                new
                {
                    parts = new[] { new { text = userPrompt } }
                }
            }
        };

        var json = JsonSerializer.Serialize(payload);
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
                var text = parts[0].GetProperty("text").GetString();
                return text ?? string.Empty;
            }
        }

        return string.Empty;
    }
}

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VenEl.Agent.Core.Interfaces;

namespace VenEl.Agent.Infrastructure.Clients;

public class OpenAiCompatibleClient : ILlmClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl;

    public OpenAiCompatibleClient(HttpClient httpClient, string apiKey, string baseUrl = "https://api.openai.com/v1")
    {
        _httpClient = httpClient;
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        _baseUrl = baseUrl.TrimEnd('/');
    }

    public async Task<string> GenerateTextAsync(string model, string systemPrompt, string userPrompt)
    {
        var url = $"{_baseUrl}/chat/completions";

        var payload = new
        {
            model = model,
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = userPrompt }
            }
        };

        var json = JsonSerializer.Serialize(payload);
        
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"API Error ({_baseUrl}): {response.StatusCode} - {responseString}");
        }

        using var doc = JsonDocument.Parse(responseString);
        var root = doc.RootElement;
        
        if (root.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
        {
            var firstChoice = choices[0];
            if (firstChoice.TryGetProperty("message", out var message) &&
                message.TryGetProperty("content", out var content))
            {
                return content.GetString() ?? string.Empty;
            }
        }

        return string.Empty;
    }
}

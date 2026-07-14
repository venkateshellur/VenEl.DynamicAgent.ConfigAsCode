using System.Threading.Tasks;
using VenEl.Agent.Core.Interfaces;

namespace VenEl.Agent.Infrastructure.Clients;

public class MockLlmClient : ILlmClient
{
    public Task<string> GenerateTextAsync(string model, string systemPrompt, string userPrompt)
    {
        // Simply echoes back for testing without an API key
        return Task.FromResult($"[MOCK {model}] Acknowledged: '{userPrompt}'. Based on my persona: '{systemPrompt}'");
    }
}

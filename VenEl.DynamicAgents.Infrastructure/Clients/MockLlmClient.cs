using System.Threading.Tasks;
using VenEl.DynamicAgents.Core.Interfaces;

namespace VenEl.DynamicAgents.Infrastructure.Clients;

public class MockLlmClient : ILlmClient
{
    public Task<string> GenerateTextAsync(string model, string systemPrompt, string userPrompt, System.Collections.Generic.IEnumerable<ITool>? tools = null)
    {
        // Simply echoes back for testing without an API key
        return Task.FromResult($"[MOCK {model}] Acknowledged: '{userPrompt}'. Based on my persona: '{systemPrompt}'");
    }
}

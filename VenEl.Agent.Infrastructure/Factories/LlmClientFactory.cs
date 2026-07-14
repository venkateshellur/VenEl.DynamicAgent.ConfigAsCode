using System;
using System.Collections.Generic;
using VenEl.Agent.Core.Interfaces;
using VenEl.Agent.Core.Models;
using VenEl.Agent.Infrastructure.Clients;

namespace VenEl.Agent.Infrastructure.Factories;

public class LlmClientFactory : ILlmClientFactory
{
    private readonly Dictionary<string, ILlmClient> _providers = new(StringComparer.OrdinalIgnoreCase);

    public void RegisterProvider(string name, ILlmClient client)
    {
        _providers[name] = client;
    }

    public ILlmClient GetClient(AgentConfig agentConfig)
    {
        // 1. Explicit Routing (User specified provider in agents.yaml)
        if (!string.IsNullOrEmpty(agentConfig.Provider))
        {
            if (_providers.TryGetValue(agentConfig.Provider, out var client))
            {
                return client;
            }
            throw new Exception($"Provider '{agentConfig.Provider}' is explicitly requested but not registered in the Factory.");
        }

        // 2. Implicit Routing (Convention over Configuration based on Model Name)
        var model = agentConfig.Model.ToLowerInvariant();
        if (model.StartsWith("gemini") && _providers.TryGetValue("google", out var googleClient))
        {
            return googleClient;
        }
        if (model.StartsWith("gpt") && _providers.TryGetValue("openai", out var openAiClient))
        {
            return openAiClient;
        }
        if (model.StartsWith("claude") && _providers.TryGetValue("anthropic", out var anthropicClient))
        {
            return anthropicClient;
        }

        // 3. Fallback to Mock if available
        if (_providers.TryGetValue("mock", out var mockClient))
        {
            return mockClient;
        }

        throw new Exception($"Could not route model '{agentConfig.Model}'. No implicit match found and no explicit provider specified.");
    }
}

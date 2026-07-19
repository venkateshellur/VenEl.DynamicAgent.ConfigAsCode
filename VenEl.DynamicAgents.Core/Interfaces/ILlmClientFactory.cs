using VenEl.DynamicAgents.Core.Models;

namespace VenEl.DynamicAgents.Core.Interfaces;

/// <summary>
/// Factory for instantiating the appropriate LLM client based on agent configuration.
/// </summary>
public interface ILlmClientFactory
{
    ILlmClient GetClient(AgentConfig agentConfig);
}

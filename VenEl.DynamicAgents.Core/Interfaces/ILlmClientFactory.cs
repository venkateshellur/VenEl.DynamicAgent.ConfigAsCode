using VenEl.DynamicAgents.Core.Models;

namespace VenEl.DynamicAgents.Core.Interfaces;

public interface ILlmClientFactory
{
    ILlmClient GetClient(AgentConfig agentConfig);
}

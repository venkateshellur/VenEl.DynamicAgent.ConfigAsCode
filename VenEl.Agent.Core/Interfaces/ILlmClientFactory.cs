using VenEl.Agent.Core.Models;

namespace VenEl.Agent.Core.Interfaces;

public interface ILlmClientFactory
{
    ILlmClient GetClient(AgentConfig agentConfig);
}

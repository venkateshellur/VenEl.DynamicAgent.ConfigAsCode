using VenEl.DynamicAgents.Core.Models;

namespace VenEl.DynamicAgents.Core.Interfaces;

/// <summary>
/// Factory for creating agent instances based on configuration types.
/// </summary>
public interface IAgentFactory
{
    /// <summary>
    /// Creates a configured agent instance.
    /// </summary>
    /// <param name="config">The configuration specifying the agent type.</param>
    /// <param name="llmClient">The LLM client to be injected into the agent.</param>
    /// <param name="logger">The logger to be injected into the agent.</param>
    /// <returns>An instance implementing IAgent.</returns>
    IAgent CreateAgent(AgentConfig config, ILlmClient llmClient, IAgentLogger logger, System.Collections.Generic.IEnumerable<ITool>? tools = null);
}

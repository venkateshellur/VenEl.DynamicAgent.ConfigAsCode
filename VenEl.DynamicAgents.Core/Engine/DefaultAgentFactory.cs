using System;
using System.Collections.Generic;
using VenEl.DynamicAgents.Core.Interfaces;
using VenEl.DynamicAgents.Core.Models;

namespace VenEl.DynamicAgents.Core.Engine;

/// <summary>
/// A default implementation of IAgentFactory that allows dynamic registration of agent types.
/// </summary>
public class DefaultAgentFactory : IAgentFactory
{
    private readonly Dictionary<string, Func<AgentConfig, ILlmClient, IAgentLogger, IEnumerable<ITool>?, IAgent>> _agentFactories 
        = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Registers a new agent type with a factory delegate.
    /// </summary>
    /// <param name="type">The string identifier for the agent type (e.g. "Standard").</param>
    /// <param name="factory">The function to create the agent.</param>
    public void RegisterAgentType(string type, Func<AgentConfig, ILlmClient, IAgentLogger, IEnumerable<ITool>?, IAgent> factory)
    {
        _agentFactories[type] = factory;
    }

    /// <summary>
    /// Creates a configured agent instance based on the configuration type.
    /// </summary>
    public IAgent CreateAgent(AgentConfig config, ILlmClient llmClient, IAgentLogger logger, System.Collections.Generic.IEnumerable<ITool>? tools = null)
    {
        if (_agentFactories.TryGetValue(config.Type, out var factory))
        {
            return factory(config, llmClient, logger, tools);
        }

        throw new NotSupportedException($"AgentType '{config.Type}' is not registered in the DefaultAgentFactory.");
    }
}

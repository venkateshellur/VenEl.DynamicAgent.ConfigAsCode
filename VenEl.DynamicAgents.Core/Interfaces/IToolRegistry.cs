using System.Collections.Generic;

namespace VenEl.DynamicAgents.Core.Interfaces;

/// <summary>
/// Central registry for dynamically resolving and providing tools to agents.
/// </summary>
public interface IToolRegistry
{
    /// <summary>
    /// Resolves and returns a collection of tools matching the given names.
    /// </summary>
    /// <param name="toolNames">The names of the tools to resolve.</param>
    /// <returns>A collection of instantiated ITool objects.</returns>
    IEnumerable<ITool> GetTools(IEnumerable<string> toolNames);
}

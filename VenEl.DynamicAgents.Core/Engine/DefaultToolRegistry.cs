using System.Collections.Generic;
using System.Linq;
using VenEl.DynamicAgents.Core.Interfaces;

namespace VenEl.DynamicAgents.Core.Engine;

public class DefaultToolRegistry : IToolRegistry
{
    private readonly IEnumerable<ITool> _availableTools;

    public DefaultToolRegistry(IEnumerable<ITool> availableTools)
    {
        _availableTools = availableTools ?? Enumerable.Empty<ITool>();
    }

    public IEnumerable<ITool> GetTools(IEnumerable<string> toolNames)
    {
        if (toolNames == null || !toolNames.Any())
            return Enumerable.Empty<ITool>();

        return _availableTools.Where(t => toolNames.Contains(t.Name));
    }
}

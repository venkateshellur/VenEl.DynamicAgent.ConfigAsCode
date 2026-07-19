using System.Collections.Generic;
using System.Threading.Tasks;
using VenEl.DynamicAgents.Core.Models;

namespace VenEl.DynamicAgents.Core.Interfaces;

/// <summary>
/// Provides configuration data for agents and workflows from an external source (e.g. YAML files).
/// </summary>
public interface IConfigurationProvider
{
    Task<IEnumerable<AgentConfig>> LoadAgentsAsync();
    Task<IEnumerable<WorkflowConfig>> LoadWorkflowsAsync();
}

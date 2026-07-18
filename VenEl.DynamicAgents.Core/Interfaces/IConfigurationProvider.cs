using System.Collections.Generic;
using System.Threading.Tasks;
using VenEl.DynamicAgents.Core.Models;

namespace VenEl.DynamicAgents.Core.Interfaces;

public interface IConfigurationProvider
{
    Task<IEnumerable<AgentConfig>> LoadAgentsAsync();
    Task<IEnumerable<WorkflowConfig>> LoadWorkflowsAsync();
}

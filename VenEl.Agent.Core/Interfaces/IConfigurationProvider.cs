using System.Collections.Generic;
using System.Threading.Tasks;
using VenEl.Agent.Core.Models;

namespace VenEl.Agent.Core.Interfaces;

public interface IConfigurationProvider
{
    Task<IEnumerable<AgentConfig>> LoadAgentsAsync();
    Task<IEnumerable<WorkflowConfig>> LoadWorkflowsAsync();
}

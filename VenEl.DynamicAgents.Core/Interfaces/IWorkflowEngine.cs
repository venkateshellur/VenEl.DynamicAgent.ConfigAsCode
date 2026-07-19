using System.Threading.Tasks;
using VenEl.DynamicAgents.Core.Models;

namespace VenEl.DynamicAgents.Core.Interfaces;

/// <summary>
/// Represents the execution engine responsible for orchestrating multi-agent workflows.
/// </summary>
public interface IWorkflowEngine
{
    Task<string> ExecuteWorkflowAsync(string workflowId, string initialInput);
}

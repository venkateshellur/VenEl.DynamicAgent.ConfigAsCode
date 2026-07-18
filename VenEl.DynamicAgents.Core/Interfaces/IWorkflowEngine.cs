using System.Threading.Tasks;
using VenEl.DynamicAgents.Core.Models;

namespace VenEl.DynamicAgents.Core.Interfaces;

public interface IWorkflowEngine
{
    Task<string> ExecuteWorkflowAsync(string workflowId, string initialInput);
}

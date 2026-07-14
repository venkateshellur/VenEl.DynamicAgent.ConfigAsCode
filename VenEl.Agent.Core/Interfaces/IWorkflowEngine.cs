using System.Threading.Tasks;
using VenEl.Agent.Core.Models;

namespace VenEl.Agent.Core.Interfaces;

public interface IWorkflowEngine
{
    Task<string> ExecuteWorkflowAsync(string workflowId, string initialInput);
}

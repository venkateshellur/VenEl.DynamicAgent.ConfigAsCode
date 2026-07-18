using System.Threading.Tasks;

namespace VenEl.DynamicAgents.Core.Interfaces;

public interface IAgentLogger
{
    Task LogPromptAsync(string agentId, string prompt);
    Task LogResponseAsync(string agentId, string response, int tokenCount);
    Task LogToolExecutionAsync(string agentId, string toolName, string arguments);
    Task LogToolResultAsync(string agentId, string toolName, string result);
    Task LogWorkflowTransitionAsync(string workflowId, string fromAgent, string toAgent);
}

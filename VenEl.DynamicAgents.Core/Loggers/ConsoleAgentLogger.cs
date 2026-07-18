using System;
using System.Threading.Tasks;
using VenEl.DynamicAgents.Core.Interfaces;

namespace VenEl.DynamicAgents.Core.Loggers;

public class ConsoleAgentLogger : IAgentLogger
{
    public Task LogPromptAsync(string agentId, string prompt)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[{agentId}] PROMPT: {prompt}");
        Console.ResetColor();
        return Task.CompletedTask;
    }

    public Task LogResponseAsync(string agentId, string response, int tokenCount)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[{agentId}] RESPONSE ({tokenCount} tokens): {response}");
        Console.ResetColor();
        return Task.CompletedTask;
    }

    public Task LogToolExecutionAsync(string agentId, string toolName, string arguments)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[{agentId}] EXECUTE TOOL '{toolName}' with args: {arguments}");
        Console.ResetColor();
        return Task.CompletedTask;
    }

    public Task LogToolResultAsync(string agentId, string toolName, string result)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"[{agentId}] TOOL '{toolName}' RESULT: {result}");
        Console.ResetColor();
        return Task.CompletedTask;
    }

    public Task LogWorkflowTransitionAsync(string workflowId, string fromAgent, string toAgent)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"[Workflow {workflowId}] TRANSITION: {fromAgent} -> {toAgent}");
        Console.ResetColor();
        return Task.CompletedTask;
    }
}

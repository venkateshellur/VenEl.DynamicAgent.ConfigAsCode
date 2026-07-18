using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VenEl.DynamicAgents.Core.Agents;
using VenEl.DynamicAgents.Core.Interfaces;
using VenEl.DynamicAgents.Core.Models;

namespace VenEl.DynamicAgents.Core.Engine;

public class SequentialWorkflowEngine : IWorkflowEngine
{
    private readonly IConfigurationProvider _configProvider;
    private readonly ILlmClientFactory _llmClientFactory;
    private readonly IAgentLogger _logger;

    public SequentialWorkflowEngine(
        IConfigurationProvider configProvider, 
        ILlmClientFactory llmClientFactory,
        IAgentLogger logger)
    {
        _configProvider = configProvider;
        _llmClientFactory = llmClientFactory;
        _logger = logger;
    }

    public async Task<string> ExecuteWorkflowAsync(string workflowId, string initialInput)
    {
        var workflows = await _configProvider.LoadWorkflowsAsync();
        var workflow = workflows.FirstOrDefault(w => w.Id == workflowId);
        
        if (workflow == null)
            throw new Exception($"Workflow {workflowId} not found.");

        var agentsConfig = await _configProvider.LoadAgentsAsync();
        var agentConfigs = agentsConfig.ToDictionary(a => a.Id);

        string currentInput = initialInput;
        var dataStore = new Dictionary<string, string>(); // To store output keys

        for (int i = 0; i < workflow.Steps.Count; i++)
        {
            var step = workflow.Steps[i];
            
            if (i > 0)
            {
                var prevAgent = workflow.Steps[i - 1].AgentId;
                await _logger.LogWorkflowTransitionAsync(workflowId, prevAgent, step.AgentId);
            }

            if (!agentConfigs.TryGetValue(step.AgentId, out var agentConfig))
                throw new Exception($"Agent {step.AgentId} not found in configuration.");

            // Basic template replacement for input {output_key}
            var resolvedInput = step.Input.Replace("{previous_output}", currentInput);
            
            foreach (var kvp in dataStore)
            {
                resolvedInput = resolvedInput.Replace($"{{{kvp.Key}}}", kvp.Value);
            }

            var agentClient = _llmClientFactory.GetClient(agentConfig);
            var agent = new ConfiguredAgent(agentConfig, agentClient, _logger);
            
            var output = await agent.ExecuteAsync(resolvedInput);
            
            if (!string.IsNullOrEmpty(step.OutputKey))
            {
                dataStore[step.OutputKey] = output;
            }

            currentInput = output;
        }

        Console.WriteLine("\nWorkflow completed successfully.\n");
        return currentInput;
    }
}

using System.Threading.Tasks;
using VenEl.DynamicAgents.Core.Interfaces;
using VenEl.DynamicAgents.Core.Models;

namespace VenEl.DynamicAgents.Core.Agents;

public class ConfiguredAgent : IAgent
{
    public AgentConfig Config { get; }
    private readonly ILlmClient _llmClient;
    private readonly IAgentLogger _logger;

    public ConfiguredAgent(AgentConfig config, ILlmClient llmClient, IAgentLogger logger)
    {
        Config = config;
        _llmClient = llmClient;
        _logger = logger;
    }

    public async Task<string> ExecuteAsync(string input)
    {
        await _logger.LogPromptAsync(Config.Id, input);
        
        // In a real scenario, we'd loop for tools here.
        var response = await _llmClient.GenerateTextAsync(Config.Model, Config.Persona, input);
        
        await _logger.LogResponseAsync(Config.Id, response, response.Length); // Fake token count
        return response;
    }
}

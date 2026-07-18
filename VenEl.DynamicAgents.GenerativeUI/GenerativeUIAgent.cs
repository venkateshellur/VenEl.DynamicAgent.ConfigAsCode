using System.Threading.Tasks;
using VenEl.DynamicAgents.Core.Interfaces;
using VenEl.DynamicAgents.Core.Models;

namespace VenEl.DynamicAgents.GenerativeUI;

public class GenerativeUIAgent : IAgent
{
    public AgentConfig Config { get; }
    private readonly ILlmClient _llmClient;
    private readonly IAgentLogger _logger;

    private const string UiPersonaSuffix = 
        "\n\nCRITICAL INSTRUCTION: You are a Generative UI Agent. " +
        "You MUST output valid, beautifully styled HTML (with inline styles or Tailwind CSS classes). " +
        "Do NOT output markdown formatting. Do NOT wrap your response in ```html. Return RAW HTML only.";

    public GenerativeUIAgent(AgentConfig config, ILlmClient llmClient, IAgentLogger logger)
    {
        Config = config;
        _llmClient = llmClient;
        _logger = logger;
    }

    public async Task<string> ExecuteAsync(string input)
    {
        await _logger.LogPromptAsync(Config.Id, $"[Generative UI Request] {input}");
        
        var specializedPersona = Config.Persona + UiPersonaSuffix;
        var response = await _llmClient.GenerateTextAsync(Config.Model, specializedPersona, input);
        
        if (response.StartsWith("```html") || response.StartsWith("```"))
        {
            response = response.Replace("```html", "").Replace("```", "").Trim();
        }

        await _logger.LogResponseAsync(Config.Id, response, response.Length);
        return response;
    }
}

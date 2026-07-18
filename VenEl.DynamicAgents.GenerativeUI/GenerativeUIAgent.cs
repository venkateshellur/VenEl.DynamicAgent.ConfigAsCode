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
        "NEVER output raw JSON data. If the user asks for a chart or graph, embed Chart.js via CDN and render it! " +
        "SAFETY CONSTRAINT [RenderOnly=True]: You are strictly a visual rendering engine. " +
        "You MUST NOT include any <form> POST actions, or network calls (CDN script tags for UI libraries are allowed). " +
        "Do NOT output markdown formatting. Do NOT wrap your response in ```html. Return RAW HTML only.";

    private readonly IEnumerable<ITool>? _tools;

    public GenerativeUIAgent(AgentConfig config, ILlmClient llmClient, IAgentLogger logger, IEnumerable<ITool>? tools = null)
    {
        Config = config;
        _llmClient = llmClient;
        _logger = logger;
        _tools = tools;
    }

    public async Task<string> ExecuteAsync(string input)
    {
        await _logger.LogPromptAsync(Config.Id, $"[Generative UI Request] {input}");
        
        var specializedPersona = Config.Persona + UiPersonaSuffix;
        var response = await _llmClient.GenerateTextAsync(Config.Model, specializedPersona, input, _tools);
        
        if (response.StartsWith("```html") || response.StartsWith("```"))
        {
            response = response.Replace("```html", "").Replace("```", "").Trim();
        }

        await _logger.LogResponseAsync(Config.Id, response, response.Length);
        return response;
    }
}

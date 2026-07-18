using System.Threading.Tasks;

namespace VenEl.DynamicAgents.Core.Interfaces;

public interface ILlmClient
{
    Task<string> GenerateTextAsync(string model, string systemPrompt, string userPrompt);
}

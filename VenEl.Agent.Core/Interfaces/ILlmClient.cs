using System.Threading.Tasks;

namespace VenEl.Agent.Core.Interfaces;

public interface ILlmClient
{
    Task<string> GenerateTextAsync(string model, string systemPrompt, string userPrompt);
}

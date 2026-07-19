using System.Threading.Tasks;
using VenEl.DynamicAgents.Core.Models;

namespace VenEl.DynamicAgents.Core.Interfaces;

/// <summary>
/// Represents an AI agent capable of executing instructions using a specific LLM and toolset.
/// </summary>
public interface IAgent
{
    AgentConfig Config { get; }
    Task<string> ExecuteAsync(string input);
}

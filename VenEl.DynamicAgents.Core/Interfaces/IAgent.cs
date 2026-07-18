using System.Threading.Tasks;
using VenEl.DynamicAgents.Core.Models;

namespace VenEl.DynamicAgents.Core.Interfaces;

public interface IAgent
{
    AgentConfig Config { get; }
    Task<string> ExecuteAsync(string input);
}

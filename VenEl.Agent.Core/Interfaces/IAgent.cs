using System.Threading.Tasks;
using VenEl.Agent.Core.Models;

namespace VenEl.Agent.Core.Interfaces;

public interface IAgent
{
    AgentConfig Config { get; }
    Task<string> ExecuteAsync(string input);
}

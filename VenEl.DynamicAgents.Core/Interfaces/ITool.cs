using System.Threading.Tasks;

namespace VenEl.DynamicAgents.Core.Interfaces;

public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<string> ExecuteAsync(string arguments);
}

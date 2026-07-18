using System.Threading.Tasks;

namespace VenEl.DynamicAgents.Core.Interfaces;

public interface ITool
{
    string Name { get; }
    string Description { get; }
    object ParametersSchema { get; }
    Task<string> ExecuteAsync(string argumentsJson);
}

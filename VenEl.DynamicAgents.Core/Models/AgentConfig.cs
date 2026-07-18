using System.Collections.Generic;

namespace VenEl.DynamicAgents.Core.Models;

public class AgentConfig
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Provider { get; set; }
    public string Model { get; set; } = string.Empty;
    public string Persona { get; set; } = string.Empty;
    public List<string> Tools { get; set; } = new();
}

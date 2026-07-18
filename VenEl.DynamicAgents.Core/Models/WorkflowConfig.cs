using System.Collections.Generic;

namespace VenEl.DynamicAgents.Core.Models;

public class WorkflowConfig
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public List<WorkflowStep> Steps { get; set; } = new();
}

public class WorkflowStep
{
    public string AgentId { get; set; } = string.Empty;
    public string Input { get; set; } = string.Empty;
    public string OutputKey { get; set; } = string.Empty;
}

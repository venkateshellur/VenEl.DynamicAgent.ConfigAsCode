using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VenEl.DynamicAgents.Core.Interfaces;
using VenEl.DynamicAgents.Core.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace VenEl.DynamicAgents.Core.Providers;

public class LocalFileConfigurationProvider : IConfigurationProvider
{
    private readonly string _agentsFilePath;
    private readonly string _workflowsFilePath;
    private readonly IDeserializer _deserializer;

    public LocalFileConfigurationProvider(string agentsFilePath, string workflowsFilePath)
    {
        _agentsFilePath = agentsFilePath;
        _workflowsFilePath = workflowsFilePath;
        _deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();
    }

    public async Task<IEnumerable<AgentConfig>> LoadAgentsAsync()
    {
        if (!File.Exists(_agentsFilePath)) return new List<AgentConfig>();
        var yaml = await File.ReadAllTextAsync(_agentsFilePath);
        var root = _deserializer.Deserialize<AgentRoot>(yaml);
        return root?.Agents ?? new List<AgentConfig>();
    }

    public async Task<IEnumerable<WorkflowConfig>> LoadWorkflowsAsync()
    {
        if (!File.Exists(_workflowsFilePath)) return new List<WorkflowConfig>();
        var yaml = await File.ReadAllTextAsync(_workflowsFilePath);
        var root = _deserializer.Deserialize<WorkflowRoot>(yaml);
        return root?.Workflows ?? new List<WorkflowConfig>();
    }
    
    private class AgentRoot
    {
        public List<AgentConfig> Agents { get; set; } = new();
    }

    private class WorkflowRoot
    {
        public List<WorkflowConfig> Workflows { get; set; } = new();
    }
}

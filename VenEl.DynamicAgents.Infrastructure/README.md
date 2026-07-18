# VenEl.DynamicAgents

The **VenEl Multi-Agent Config-As-Code Framework** is a powerful .NET engine that allows you to orchestrate intelligent AI agents directly from YAML configuration files. It completely separates your business logic (agent orchestration) from your application logic.

💻 **Source Code:** [https://github.com/venkateshellur/VenEl.DynamicAgents](https://github.com/venkateshellur/VenEl.DynamicAgents)

## Features
- **Config-As-Code**: Define your agents and workflows in YAML, not C#.
- **Clean Architecture**: The Core engine is completely decoupled from the Infrastructure.
- **Multi-Provider**: Native support for Google Gemini and Puter (OpenAI compatible) out of the box, with a Mock fallback for testing.
- **Extensible**: Easily inject your own `ILlmClient` implementations for Anthropic, LLaMA, or custom inference endpoints.

## Quick Setup

### 1. Install the Package
```bash
dotnet add package VenEl.DynamicAgents.Infrastructure
```

### 2. Configure Your Configuration Files
Create `agents.yaml` and `workflows.yaml` in your project root and ensure they are copied to the output directory (`PreserveNewest`). 

**`agents.yaml`**
```yaml
agents:
  software_architect:
    name: "Architect"
    systemPrompt: "You are a senior software architect."
    modelConfig:
      provider: "google" 
```

**`workflows.yaml`**
```yaml
workflows:
  my_pipeline:
    description: "Sample pipeline"
    steps:
      - id: 1
        agentName: "software_architect"
        taskTemplate: "Design a solution for: {input}"
```

### 3. Wire Up Dependency Injection (`Program.cs`)
```csharp
var services = new ServiceCollection();

// Setup LLM Clients
services.AddSingleton<HttpClient>();
services.AddSingleton<ILlmClientFactory>(sp => 
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    var factory = new LlmClientFactory();
    
    // Register Google Gemini using an environment variable
    factory.RegisterProvider("google", new GeminiLlmClient(httpClient, Environment.GetEnvironmentVariable("GEMINI_API_KEY")));
    
    return factory;
});

// Setup Configuration Provider
services.AddSingleton<IConfigurationProvider>(new LocalFileConfigurationProvider("agents.yaml", "workflows.yaml"));

// Register the Workflow Engine
services.AddSingleton<IWorkflowEngine, SequentialWorkflowEngine>();

var serviceProvider = services.BuildServiceProvider();
var engine = serviceProvider.GetRequiredService<IWorkflowEngine>();

// Execute your pipeline!
var result = await engine.ExecuteWorkflowAsync("my_pipeline", "Build a microservices architecture.");
Console.WriteLine(result);
```

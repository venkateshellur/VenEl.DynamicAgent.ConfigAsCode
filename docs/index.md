# VenEl Multi-Agent Config-As-Code Framework

Welcome to the official documentation for **VenEl.DynamicAgents.ConfigAsCode**, a modular, config-driven agent architecture in C# (.NET 10) that allows you to rapidly build, orchestrate, and deploy AI agents utilizing an intuitive "Config-as-Code" methodology.

This guide is designed for both human developers and AI assistants to quickly scaffold a new project using the framework.

---

## 1. Installation

To start using the framework, install the core and infrastructure packages from NuGet:

```bash
# Core framework abstractions and engine
dotnet add package VenEl.DynamicAgents.Core

# Infrastructure integrations (Gemini, OpenAI, Mock clients)
dotnet add package VenEl.DynamicAgents.Infrastructure

# (Optional) Generative UI rendering agent
dotnet add package VenEl.DynamicAgents.GenerativeUI
```

---

## 2. Configuration (Config-As-Code)

The core philosophy of this framework is to separate agent definitions from application logic. Create the following YAML files in your project directory (ensure they are copied to the output directory).

### `agents.yaml`
Defines your AI personas, their configurations, and their tools.

```yaml
- id: "researcher_agent"
  name: "Deep Research Agent"
  systemPrompt: "You are an expert researcher. Synthesize information clearly."
  llmProvider: "Gemini" # Options: Gemini, OpenAiCompatible, Mock
  modelName: "gemini-2.5-flash"
  temperature: 0.3
  maxTokens: 2000
  tools: []
```

### `workflows.yaml`
Defines how your agents pass data to each other in a sequential pipeline.

```yaml
- id: "research_and_summarize"
  name: "Research and Summarization Pipeline"
  steps:
    - agentId: "researcher_agent"
      input: "{previous_output}"
      outputKey: "research_data"
```

---

## 3. Implementation (Dependency Injection)

The framework is built heavily on Microsoft's `Microsoft.Extensions.DependencyInjection`. To initialize the framework, register its services in your `Program.cs` or `Startup.cs`:

```csharp
using Microsoft.Extensions.DependencyInjection;
using VenEl.DynamicAgents.Core.Interfaces;
using VenEl.DynamicAgents.Core.Engine;
using VenEl.DynamicAgents.Core.Providers;
using VenEl.DynamicAgents.Infrastructure.Factories;
using VenEl.DynamicAgents.Core.Loggers;

var services = new ServiceCollection();

// 1. Register Configuration Provider (points to your YAML files)
services.AddSingleton<IConfigurationProvider>(new LocalFileConfigurationProvider("agents.yaml", "workflows.yaml"));

// 2. Register LLM Factory and Agent Factory
services.AddSingleton<ILlmClientFactory, LlmClientFactory>();
services.AddSingleton<IAgentFactory, DefaultAgentFactory>();

// 3. Register Tool Registry
services.AddSingleton<IToolRegistry, DefaultToolRegistry>();

// 4. Register Logger
services.AddSingleton<IAgentLogger, ConsoleAgentLogger>();

// 5. Register the Workflow Engine
services.AddSingleton<IWorkflowEngine, SequentialWorkflowEngine>();

var serviceProvider = services.BuildServiceProvider();
```

---

## 4. Execution

To execute a workflow, resolve the `IWorkflowEngine` and call `ExecuteWorkflowAsync`:

```csharp
var engine = serviceProvider.GetRequiredService<IWorkflowEngine>();

// Execute the workflow defined in workflows.yaml
string finalResult = await engine.ExecuteWorkflowAsync(
    workflowId: "research_and_summarize", 
    initialInput: "What are the latest advancements in quantum computing?"
);

Console.WriteLine(finalResult);
```

---

## Environment Variables

Ensure you have the appropriate API keys set in your environment before running the application:

- `GEMINI_API_KEY`: Required if using `llmProvider: "Gemini"`
- `OPENAI_API_KEY`: Required if using `llmProvider: "OpenAiCompatible"`

---

## Next Steps
Navigate through the **API Reference** on the left to explore the underlying classes, interfaces, and architecture in detail!

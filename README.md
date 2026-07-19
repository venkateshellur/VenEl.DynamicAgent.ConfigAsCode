# VenEl Multi-Agent Config-As-Code Framework

📚 **Official Documentation:** [https://venkateshellur.github.io/VenEl.DynamicAgent.ConfigAsCode/](https://venkateshellur.github.io/VenEl.DynamicAgent.ConfigAsCode/)


Welcome to the **VenEl Dynamic Agents Framework**! This is a modular, config-driven agent architecture in C# (.NET 10) that allows you to rapidly build, orchestrate, and deploy AI agents utilizing an intuitive "Config-as-Code" methodology.

## Getting Started

### 1. Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### 2. API Keys Setup
This framework securely loads API keys from your environment variables to prevent accidental leaks. Before running the application, you **must** configure the following environment variables:

- **Google Gemini**: Set `GEMINI_API_KEY` to your Google AI Studio API key.
- **Puter (OpenAI Compatible)**: Set `PUTER_API_KEY` to your Puter API key (if you plan to use `gpt-4o` or Puter-backed models).

**On macOS/Linux:**
```bash
export GEMINI_API_KEY="your-gemini-api-key"
export PUTER_API_KEY="your-puter-api-key"
```

**On Windows (PowerShell):**
```powershell
$env:GEMINI_API_KEY="your-gemini-api-key"
$env:PUTER_API_KEY="your-puter-api-key"
```

### 3. Running the Projects

**Web Demo (Generative UI):**
```bash
cd VenEl.DynamicAgents.Demo.Web
dotnet run
```
Navigate to `http://localhost:5201` in your browser.

**Console Demo (Multi-Agent Workflows):**
```bash
cd VenEl.DynamicAgents.Demo.Console
dotnet run
```

## 4. Integration Quickstart (Config-As-Code)

This framework is built heavily on Microsoft's `Microsoft.Extensions.DependencyInjection`.

### Step 1: Install NuGet Packages
```bash
dotnet add package VenEl.DynamicAgents.Core
dotnet add package VenEl.DynamicAgents.Infrastructure
```

### Step 2: Define your Agents (`agents.yaml`)
```yaml
- id: "researcher_agent"
  name: "Deep Research Agent"
  systemPrompt: "You are an expert researcher. Synthesize information clearly."
  llmProvider: "Gemini" 
  modelName: "gemini-2.5-flash"
  temperature: 0.3
  maxTokens: 2000
```

### Step 3: Define your Workflows (`workflows.yaml`)
```yaml
- id: "research_and_summarize"
  name: "Research and Summarization Pipeline"
  steps:
    - agentId: "researcher_agent"
      input: "{previous_output}"
      outputKey: "research_data"
```

### Step 4: Register Services & Execute (`Program.cs`)
```csharp
using Microsoft.Extensions.DependencyInjection;
using VenEl.DynamicAgents.Core.Interfaces;
using VenEl.DynamicAgents.Core.Engine;
using VenEl.DynamicAgents.Core.Providers;
using VenEl.DynamicAgents.Infrastructure.Factories;
using VenEl.DynamicAgents.Core.Loggers;

var services = new ServiceCollection();

// Register Configuration Provider
services.AddSingleton<IConfigurationProvider>(new LocalFileConfigurationProvider("agents.yaml", "workflows.yaml"));

// Register Core Services
services.AddSingleton<ILlmClientFactory, LlmClientFactory>();
services.AddSingleton<IAgentFactory, DefaultAgentFactory>();
services.AddSingleton<IToolRegistry, DefaultToolRegistry>();
services.AddSingleton<IAgentLogger, ConsoleAgentLogger>();
services.AddSingleton<IWorkflowEngine, SequentialWorkflowEngine>();

var serviceProvider = services.BuildServiceProvider();
var engine = serviceProvider.GetRequiredService<IWorkflowEngine>();

// Execute the workflow
string result = await engine.ExecuteWorkflowAsync("research_and_summarize", "Latest advancements in quantum computing?");
Console.WriteLine(result);
```

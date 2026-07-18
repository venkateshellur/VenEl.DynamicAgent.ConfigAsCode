# VenEl Dynamic Agents Framework

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

## Architecture

- **VenEl.DynamicAgents.Core**: Contains all fundamental abstractions (`IAgent`, `ITool`, `ILlmClient`, `SequentialWorkflowEngine`).
- **VenEl.DynamicAgents.Infrastructure**: The implementations for specific providers (`GeminiLlmClient`, `OpenAiCompatibleClient`).
- **VenEl.DynamicAgents.GenerativeUI**: Specialized agents designed to return HTML/Tailwind representations for web frontends.

## Configuring Agents (agents.yaml)
Agents are dynamically instantiated based on your configuration files. No need to re-compile to tweak prompts! Simply open `agents.yaml` and modify personas, models, or attached tools.

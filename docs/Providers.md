# LLM Provider Configuration

VenEl.DynamicAgents supports a **Hybrid Routing System** ("Implicit by default, Explicit fallback") to map your agents to the correct underlying LLM API provider.

## 1. Implicit Routing (Convention over Configuration)

By default, the engine will inspect the `model` string provided in your `agents.yaml` file. Based on the prefix of the model name, it automatically routes the request to the correct registered provider.

| Model Prefix | Inferred Provider | Example Model Strings |
| :--- | :--- | :--- |
| `gemini` | `google` | `gemini-2.5-flash`, `gemini-1.5-pro` |
| `gpt` | `openai` | `gpt-4o`, `gpt-3.5-turbo` |
| `claude` | `anthropic` | `claude-3-opus`, `claude-3-sonnet` |

**Example:**
```yaml
agents:
  - id: "developer_agent"
    name: "Developer"
    model: "gemini-2.5-flash"  # <--- Automatically routes to Google API
    persona: "You write code."
    tools: []
```

## 2. Explicit Routing (Fallback)

If you are using a model name that doesn't fit the standard prefixes (e.g., an open-source model like `llama-3-8b` hosted on a custom provider), or you simply want to be explicit, you can provide the `provider` property directly in the YAML configuration.

If the `provider` property is set, **Implicit Routing is skipped entirely**.

### Supported Explicit Providers:

| Provider String | Description |
| :--- | :--- |
| `"google"` | Routes to Google Gemini API or Vertex AI (depending on your registered client). |
| `"openai"` | Routes to OpenAI API. |
| `"anthropic"` | Routes to Anthropic API. |
| `"mock"` | Routes to the internal Mock Client (useful for offline testing or when API keys are missing). |

**Example:**
```yaml
agents:
  - id: "reviewer_agent"
    name: "Architect"
    provider: "openai"       # <--- Forces routing to OpenAI client
    model: "llama-3-8b"
    persona: "You review code."
    tools: []
```

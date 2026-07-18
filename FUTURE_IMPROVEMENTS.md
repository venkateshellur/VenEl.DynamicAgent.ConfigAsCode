# Future Improvements & Roadmap

This document tracks the current limitations of the **VenEl.DynamicAgents.ConfigAsCode** MVP and outlines the planned roadmap for future enhancements.

## 1. Tool Calling (Function Calling)
*   **Current State:** Agents are restricted to text-based analysis and generation. They cannot interact with external systems.
*   **Goal:** Implement the ability for agents to execute local C# functions, search the web, query databases, or call external REST APIs to fulfill their personas dynamically.

## 2. Advanced Workflow State Machine
*   **Current State:** The `SequentialWorkflowEngine` only supports strictly linear workflows (Agent A -> Agent B -> Agent C).
*   **Goal:** Build a robust state machine engine that supports:
    *   **Conditional Branching (If/Else):** e.g., If the Reviewer Agent rejects the code, send it back to the Developer Agent.
    *   **Parallel Execution:** e.g., Have two agents research different topics simultaneously and merge their findings.
    *   **Loops & Cycles:** e.g., Retry a step up to 3 times if validation fails.

## 3. State Management & Memory
*   **Current State:** Agents have amnesia. They execute in a vacuum and only see the static system prompt and the exact `{previous_output}`.
*   **Goal:** Implement a `MemoryStore` or `MessageHistory` context object that is passed along with the workflow state, giving agents access to the full conversation history.

## 4. UI Real-Time Streaming
*   **Current State:** The `POST /api/workflow/execute` endpoint blocks until the entire workflow is finished, leaving the UI hanging on a loading spinner.
*   **Goal:** Transition the endpoint to use **Server-Sent Events (SSE)** or **WebSockets**. This will allow the backend to stream real-time status updates (e.g., "Requirements Agent finished...", "Architect Agent thinking...") and stream the final text output chunk-by-chunk to the UI.

## 5. Configuration Standard Compliance
*   **Current State:** We are manually parsing `agents.yaml` and `workflows.yaml` using a custom `IConfigurationProvider` and manually reading environment variables for API keys.
*   **Goal:** Fully integrate our YAML configuration into the standard ASP.NET Core `builder.Configuration` pipeline and use the strongly-typed Options pattern (`IOptions<T>`) for dependency injection.

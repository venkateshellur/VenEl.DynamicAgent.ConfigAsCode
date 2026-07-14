using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using VenEl.Agent.Core.Engine;
using VenEl.Agent.Core.Interfaces;
using VenEl.Agent.Core.Loggers;
using VenEl.Agent.Core.Providers;
using VenEl.Agent.Infrastructure.Clients;
using VenEl.Agent.Infrastructure.Factories;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IAgentLogger, ConsoleAgentLogger>();
builder.Services.AddSingleton<HttpClient>();

builder.Services.AddSingleton<ILlmClientFactory>(sp => 
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    var factory = new LlmClientFactory();

    factory.RegisterProvider("mock", new MockLlmClient());

    var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
    if (!string.IsNullOrEmpty(apiKey))
    {
        factory.RegisterProvider("google", new GeminiLlmClient(httpClient, apiKey));
    }

    var puterKey = Environment.GetEnvironmentVariable("PUTER_API_KEY");
    if (!string.IsNullOrEmpty(puterKey))
    {
        factory.RegisterProvider("puter", new OpenAiCompatibleClient(httpClient, puterKey, "https://api.puter.com/puterai/openai/v1"));
    }

    return factory;
});

var baseDir = AppDomain.CurrentDomain.BaseDirectory;
builder.Services.AddSingleton<VenEl.Agent.Core.Interfaces.IConfigurationProvider>(new LocalFileConfigurationProvider(
    Path.Combine(baseDir, "agents.yaml"), 
    Path.Combine(baseDir, "workflows.yaml")));
    
builder.Services.AddSingleton<IWorkflowEngine, SequentialWorkflowEngine>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Enable Static Files for the UI
app.UseDefaultFiles();
app.UseStaticFiles();

// Minimal API Endpoint
app.MapPost("/api/workflow/execute", async (WorkflowRequest req, IWorkflowEngine engine) =>
{
    try
    {
        var result = await engine.ExecuteWorkflowAsync(req.WorkflowId, req.Input);
        return Results.Ok(new { Output = result });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { Error = ex.Message });
    }
})
.WithName("ExecuteWorkflow");

app.Run();

public record WorkflowRequest(string WorkflowId, string Input);

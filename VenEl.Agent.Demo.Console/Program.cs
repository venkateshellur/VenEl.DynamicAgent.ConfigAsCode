using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using VenEl.Agent.Core.Engine;
using VenEl.Agent.Core.Interfaces;
using VenEl.Agent.Core.Loggers;
using VenEl.Agent.Core.Providers;
using VenEl.Agent.Infrastructure.Clients;
using VenEl.Agent.Infrastructure.Factories;

namespace VenEl.Agent.Demo.ConsoleApp;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting VenEl Agent Framework...\n");

        var services = new ServiceCollection();
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        // Wire up Core Interfaces
        services.AddSingleton<IAgentLogger, ConsoleAgentLogger>();
        services.AddSingleton<HttpClient>();
        
        services.AddSingleton<ILlmClientFactory>(sp => 
        {
            var httpClient = sp.GetRequiredService<HttpClient>();
            var factory = new LlmClientFactory();

            // Always register Mock Provider as fallback
            factory.RegisterProvider("mock", new MockLlmClient());

            var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
            if (!string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("Gemini API Key detected! Registering Google provider.");
                factory.RegisterProvider("google", new GeminiLlmClient(httpClient, apiKey));
            }
            else
            {
                Console.WriteLine("WARNING: GEMINI_API_KEY not found. Google agents will fallback to Mock.");
            }

            var puterKey = Environment.GetEnvironmentVariable("PUTER_API_KEY");
            if (!string.IsNullOrEmpty(puterKey))
            {
                Console.WriteLine("Puter API Key detected! Registering Puter provider.");
                factory.RegisterProvider("puter", new OpenAiCompatibleClient(httpClient, puterKey, "https://api.puter.com/puterai/openai/v1"));
            }
            else
            {
                Console.WriteLine("WARNING: PUTER_API_KEY not found. Puter agents will fallback to Mock.");
            }

            return factory;
        });

        services.AddSingleton<IConfigurationProvider>(new LocalFileConfigurationProvider(
            Path.Combine(baseDir, "agents.yaml"), 
            Path.Combine(baseDir, "workflows.yaml")));
        services.AddSingleton<IWorkflowEngine, SequentialWorkflowEngine>();

        var serviceProvider = services.BuildServiceProvider();
        var engine = serviceProvider.GetRequiredService<IWorkflowEngine>();

        // Run Workflow 1
        Console.WriteLine("================================================");
        Console.WriteLine("Executing Workflow 1: code_review_pipeline");
        Console.WriteLine("================================================\n");
        await engine.ExecuteWorkflowAsync("code_review_pipeline", "Write a C# method to reverse a string.");

        // Run Workflow 2
        Console.WriteLine("\n\n================================================");
        Console.WriteLine("Executing Workflow 2: travel_planner_pipeline");
        Console.WriteLine("================================================\n");
        
        await engine.ExecuteWorkflowAsync("travel_planner_pipeline", "Tokyo, Japan");

        Console.WriteLine("\n------------------------------------------------");
        Console.WriteLine("Both Workflows completed successfully!");
    }
}

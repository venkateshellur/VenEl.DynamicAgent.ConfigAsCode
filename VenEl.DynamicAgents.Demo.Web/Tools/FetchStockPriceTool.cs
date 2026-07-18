using System;
using System.Text.Json;
using System.Threading.Tasks;
using VenEl.DynamicAgents.Core.Interfaces;

namespace VenEl.DynamicAgents.Demo.Web.Tools;

public class FetchStockPriceTool : ITool
{
    public string Name => "fetch_stock_price";

    public string Description => "Fetches the daily closing price for a given stock ticker symbol for the last 7 days.";

    public object ParametersSchema => new
    {
        type = "object",
        properties = new
        {
            ticker = new
            {
                type = "string",
                description = "The stock ticker symbol (e.g., AAPL, TSLA, MSTY)."
            }
        },
        required = new[] { "ticker" }
    };

    public Task<string> ExecuteAsync(string argumentsJson)
    {
        string ticker = "UNKNOWN";
        try
        {
            using var doc = JsonDocument.Parse(argumentsJson);
            if (doc.RootElement.TryGetProperty("ticker", out var tickerElement))
            {
                ticker = tickerElement.GetString() ?? "UNKNOWN";
            }
        }
        catch
        {
            // Ignore parsing errors for mock
        }

        // Mocking real stock data for the demonstration
        var random = new Random();
        var basePrice = ticker == "MSTY" ? 25.0 : 150.0;
        
        var data = new
        {
            ticker = ticker.ToUpper(),
            days = 7,
            prices = new double[7]
        };

        for (int i = 0; i < 7; i++)
        {
            data.prices[i] = Math.Round(basePrice + (random.NextDouble() * 5 - 2.5), 2);
        }

        return Task.FromResult(JsonSerializer.Serialize(data));
    }
}

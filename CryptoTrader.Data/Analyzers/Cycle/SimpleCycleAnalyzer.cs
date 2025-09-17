using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class SimpleCycleAnalyzer : StockDataAnalyzerBase<SimpleCycleAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateSimpleCycle(settings.Length);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Sc", results.OutputValues["Sc"] }
            };
        }

        public class Settings
        {
            public int Length { get; set; } = 50;
        }
    }
}

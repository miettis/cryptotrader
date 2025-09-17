using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class EhlersStochasticCyberCycleAnalyzer : StockDataAnalyzerBase<EhlersStochasticCyberCycleAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateEhlersStochasticCyberCycle(settings.Length, settings.Alpha);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Escc", results.OutputValues["Escc"] }
            };
        }

        public class Settings
        {
            public int Length { get; set; } = 14;
            public double Alpha { get; set; } = 0.7;
        }
    }
}

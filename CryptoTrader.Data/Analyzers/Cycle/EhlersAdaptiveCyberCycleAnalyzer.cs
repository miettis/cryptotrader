using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class EhlersAdaptiveCyberCycleAnalyzer : StockDataAnalyzerBase<EhlersAdaptiveCyberCycleAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateEhlersAdaptiveCyberCycle(settings.Length, settings.Alpha);
            return results.OutputValues;
        }

        public class Settings
        {
            public int Length { get; set; } = 5;
            public double Alpha { get; set; } = 0.07;
        }
    }
}

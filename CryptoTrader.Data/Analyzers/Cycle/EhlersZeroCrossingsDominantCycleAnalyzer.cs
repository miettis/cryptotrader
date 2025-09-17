using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class EhlersZeroCrossingsDominantCycleAnalyzer : StockDataAnalyzerBase<EhlersZeroCrossingsDominantCycleAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateEhlersZeroCrossingsDominantCycle(settings.Length, settings.Alpha);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Ezcdc", results.OutputValues["Ezcdc"] }
            };
        }

        public class Settings
        {
            public int Length { get; set; } = 20;
            public double Alpha { get; set; } = 0.7;
        }
    }
}

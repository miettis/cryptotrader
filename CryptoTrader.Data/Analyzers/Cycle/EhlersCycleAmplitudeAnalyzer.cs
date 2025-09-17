using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class EhlersCycleAmplitudeAnalyzer : StockDataAnalyzerBase<EhlersCycleAmplitudeAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateEhlersCycleAmplitude(settings.Length, settings.Delta);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Eca", results.OutputValues["Eca"] }
            };
        }

        public class Settings
        {
            public int Length { get; set; } = 20;
            public double Delta { get; set; } = 0.1;
        }
    }
}

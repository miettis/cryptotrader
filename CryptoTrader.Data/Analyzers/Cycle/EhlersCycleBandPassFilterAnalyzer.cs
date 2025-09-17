using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class EhlersCycleBandPassFilterAnalyzer : StockDataAnalyzerBase<EhlersCycleBandPassFilterAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateEhlersCycleBandPassFilter(settings.Length, settings.Delta);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Ecbpf", results.OutputValues["Ecbpf"] }
            };
        }

        public class Settings
        {
            public int Length { get; set; } = 20;
            public double Delta { get; set; } = 0.1;
        }
    }
}

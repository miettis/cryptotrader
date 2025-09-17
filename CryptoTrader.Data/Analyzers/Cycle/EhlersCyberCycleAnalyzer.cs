using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class EhlersCyberCycleAnalyzer : StockDataAnalyzerBase<EhlersCyberCycleAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateEhlersCyberCycle(settings.Alpha);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Ecc", results.OutputValues["Ecc"] }
            };
        }

        public class Settings
        {
            public double Alpha { get; set; } = 0.07;
        }
    }
}

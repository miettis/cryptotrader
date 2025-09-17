using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class EhlersCombFilterSpectralEstimateAnalyzer : StockDataAnalyzerBase<EhlersCombFilterSpectralEstimateAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateEhlersCombFilterSpectralEstimate(settings.Length1, settings.Length2, settings.Bw);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Ecfse", results.OutputValues["Ecse"] }
            };
        }

        public class Settings
        {
            public int Length1 { get; set; } = 48;
            public int Length2 { get; set; } = 10;
            public double Bw { get; set; } = 0.3;
        }
    }
}

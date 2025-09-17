using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class EhlersEvenBetterSineWaveIndicatorAnalyzer : StockDataAnalyzerBase<EhlersEvenBetterSineWaveIndicatorAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateEhlersEvenBetterSineWaveIndicator(settings.Length1, settings.Length2);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Eebswi", results.OutputValues["Eebswi"] }
            };
        }

        public class Settings
        {
            public int Length1 { get; set; } = 40;
            public int Length2 { get; set; } = 10;
        }
    }
}

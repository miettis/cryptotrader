using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class EhlersInstantaneousPhaseIndicatorAnalyzer : StockDataAnalyzerBase<EhlersInstantaneousPhaseIndicatorAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateEhlersInstantaneousPhaseIndicator(settings.Length1, settings.Length2);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Eipi", results.OutputValues["Eipi"] }
            };
        }

        public class Settings
        {
            public int Length1 { get; set; } = 7;
            public int Length2 { get; set; } = 50;
        }
    }
}

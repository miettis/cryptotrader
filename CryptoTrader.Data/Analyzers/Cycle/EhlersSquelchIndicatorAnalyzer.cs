using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class EhlersSquelchIndicatorAnalyzer : StockDataAnalyzerBase<EhlersSquelchIndicatorAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateEhlersSquelchIndicator(settings.Length1, settings.Length2, settings.Length3);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Esi", results.OutputValues["Esi"] }
            };
        }

        public class Settings
        {
            public int Length1 { get; set; } = 6;
            public int Length2 { get; set; } = 20;
            public int Length3 { get; set; } = 40;
        }
    }
}

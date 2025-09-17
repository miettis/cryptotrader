using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class EhlersSimpleCycleIndicatorAnalyzer : StockDataAnalyzerBase<EhlersSimpleCycleIndicatorAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateEhlersSimpleCycleIndicator(settings.Alpha);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Esci", results.OutputValues["Esci"] }
            };
        }

        public class Settings
        {
            public double Alpha { get; set; } = 0.07;
        }
    }
}

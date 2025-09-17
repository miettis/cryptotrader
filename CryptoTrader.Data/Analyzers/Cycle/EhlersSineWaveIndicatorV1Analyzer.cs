using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class EhlersSineWaveIndicatorV1Analyzer : StockDataAnalyzerBase<EhlersSineWaveIndicatorV1Analyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateEhlersSineWaveIndicatorV1();
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Eswiv1", results.OutputValues["Eswiv1"] }
            };
        }

        public class Settings
        {
        }
    }
}

using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class EhlersSineWaveIndicatorV2Analyzer : StockDataAnalyzerBase<EhlersSineWaveIndicatorV2Analyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateEhlersSineWaveIndicatorV2(settings.Length, settings.Alpha);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Eswiv2", results.OutputValues["Eswiv2"] }
            };
        }

        public class Settings
        {
            public int Length { get; set; } = 5;
            public double Alpha { get; set; } = 0.07;
        }
    }
}

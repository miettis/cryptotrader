using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class EhlersFourierSeriesAnalysisAnalyzer : StockDataAnalyzerBase<EhlersFourierSeriesAnalysisAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateEhlersFourierSeriesAnalysis(settings.Length, settings.Bw);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Efsa", results.OutputValues["Efsa"] }
            };
        }

        public class Settings
        {
            public int Length { get; set; } = 20;
            public double Bw { get; set; } = 0.1;
        }
    }
}

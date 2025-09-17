// EhlersDualDifferentiatorDominantCycleAnalyzer.cs
using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class EhlersDualDifferentiatorDominantCycleAnalyzer : StockDataAnalyzerBase<EhlersDualDifferentiatorDominantCycleAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateEhlersDualDifferentiatorDominantCycle(settings.Length1, settings.Length2, settings.Length3);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Edddc", results.OutputValues["Edddc"] }
            };
        }

        public class Settings
        {
            public int Length1 { get; set; } = 48;
            public int Length2 { get; set; } = 20;
            public int Length3 { get; set; } = 8;
        }
    }
}

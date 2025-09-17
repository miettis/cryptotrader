using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class EhlersPhaseAccumulationDominantCycleAnalyzer : StockDataAnalyzerBase<EhlersPhaseAccumulationDominantCycleAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateEhlersPhaseAccumulationDominantCycle(settings.Length1, settings.Length2, settings.Length3, settings.Length4);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Epadc", results.OutputValues["Epadc"] }
            };
        }

        public class Settings
        {
            public int Length1 { get; set; } = 48;
            public int Length2 { get; set; } = 20;
            public int Length3 { get; set; } = 10;
            public int Length4 { get; set; } = 40;
        }
    }
}

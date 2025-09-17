using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Enums;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class GroverLlorensCycleOscillatorAnalyzer : StockDataAnalyzerBase<GroverLlorensCycleOscillatorAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateGroverLlorensCycleOscillator(settings.MaType, settings.Length, settings.SmoothLength, settings.Mult);
            return results.OutputValues;
            return new Dictionary<string, List<double>>
            {
                {"Glco", results.OutputValues["Glco"] }
            };
        }

        public class Settings
        {
            public MovingAvgType MaType { get; set; } = MovingAvgType.WildersSmoothingMethod;
            public int Length { get; set; } = 100;
            public int SmoothLength { get; set; } = 20;
            public double Mult { get; set; } = 10;
        }
    }
}

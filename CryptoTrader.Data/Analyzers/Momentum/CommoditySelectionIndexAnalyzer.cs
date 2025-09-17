using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Enums;
using OoplesFinance.StockIndicators.Models;

namespace CryptoTrader.Data.Analyzers
{
    public class CommoditySelectionIndexAnalyzer : StockDataAnalyzerBase<CommoditySelectionIndexAnalyzer.Settings>
    {
        public override Dictionary<string, List<double>> Analyze(StockData data, Settings settings)
        {
            var results = data.CalculateCommoditySelectionIndex(settings.MaType, settings.Length, settings.PointValue, settings.Margin, settings.Commission);
            return new Dictionary<string, List<double>>
            {
                {"Csi", results.OutputValues["Csi"] }
            };
        }

        public class Settings
        {
            public MovingAvgType MaType { get; set; } = MovingAvgType.WildersSmoothingMethod;
            public int Length { get; set; } = 14;
            public double PointValue { get; set; } = 50;
            public double Margin { get; set; } = 3000;
            public double Commission { get; set; } = 10;
        }
    }
}

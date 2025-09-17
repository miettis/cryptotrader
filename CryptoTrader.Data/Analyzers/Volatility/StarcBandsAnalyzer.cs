using CryptoTrader.Data.Features;
using OoplesFinance.StockIndicators.Models;
using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class StarcBandsAnalyzer : SkendrAnalyzerBase<StarcBandsAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetStarcBands(settings.SmaPeriods, settings.Multiplier, settings.AtrPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"UpperBand", result.Select(x => x.UpperBand).ToList() },
                {"Centerline", result.Select(x => x.Centerline).ToList() },
                {"LowerBand", result.Select(x => x.LowerBand).ToList() }
            };
        }

        public class Settings
        {
            public int SmaPeriods { get; set; } = 20;
            public double Multiplier { get; set; } = 2.0;
            public int AtrPeriods { get; set; } = 10;
        }
    }
}

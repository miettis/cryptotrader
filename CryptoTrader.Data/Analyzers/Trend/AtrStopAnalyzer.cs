using CryptoTrader.Data.Features;
using OoplesFinance.StockIndicators.Models;
using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class AtrStopAnalyzer : SkendrAnalyzerBase<AtrStopAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetAtrStop(settings.LookbackPeriods, settings.Multiplier, settings.EndType);
            return new Dictionary<string, List<double?>>
            {
                {"AtrStop", result.Select(x => (double?)x.AtrStop).ToList() },
                {"BuyStop", result.Select(x => (double?)x.BuyStop).ToList() },
                {"SellStop", result.Select(x => (double?)x.SellStop).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 21;
            public double Multiplier { get; set; } = 3.0;
            public EndType EndType { get; set; } = EndType.Close;
        }
    }
}

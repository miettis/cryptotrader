using CryptoTrader.Data.Features;
using OoplesFinance.StockIndicators.Models;
using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class ForceIndexAnalyzer : SkendrAnalyzerBase<ForceIndexAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetForceIndex(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"ForceIndex", result.Select(x => x.ForceIndex).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 2;
        }
    }
}

using CryptoTrader.Data.Features;
using OoplesFinance.StockIndicators.Models;
using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class ChopAnalyzer : SkendrAnalyzerBase<ChopAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetChop(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Chop", result.Select(x => x.Chop).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 14;
        }
    }
}

using CryptoTrader.Data.Features;
using OoplesFinance.StockIndicators.Models;
using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class HtTrendlineAnalyzer : SkendrAnalyzerBase<HtTrendlineAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetHtTrendline();
            return new Dictionary<string, List<double?>>
            {
                {"Trendline", result.Select(x => x.Trendline).ToList() },
                {"SmoothPrice", result.Select(x => x.SmoothPrice).ToList() },
                {"DcPeriods", result.Select(x => (double?)x.DcPeriods).ToList() }
            };
        }

        public class Settings { }
    }
}

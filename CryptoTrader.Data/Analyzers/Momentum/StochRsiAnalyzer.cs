using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class StochRsiAnalyzer : SkendrAnalyzerBase<StochRsiAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetStochRsi(settings.RsiPeriods, settings.StochPeriods, settings.SignalPeriods, settings.SmoothPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"StockRsi", result.Select(x => x.StochRsi).ToList() },
                {"Signal", result.Select(x => x.Signal).ToList() }
            };
        }

        public class Settings
        {
            public int RsiPeriods { get; set; } = 14;
            public int StochPeriods { get; set; } = 14;
            public int SignalPeriods { get; set; } = 3;
            public int SmoothPeriods { get; set; } = 1;
        }
    }
}

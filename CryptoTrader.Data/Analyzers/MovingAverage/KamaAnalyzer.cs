using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class KamaAnalyzer : SkendrAnalyzerBase<KamaAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetKama(settings.ErPeriods, settings.FastPeriods, settings.SlowPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Kama", result.Select(x => x.Kama).ToList() }
            };
        }

        public class Settings
        {
            public int ErPeriods { get; set; } = 10;
            public int FastPeriods { get; set; } = 2;
            public int SlowPeriods { get; set; } = 30;
        }
    }
}

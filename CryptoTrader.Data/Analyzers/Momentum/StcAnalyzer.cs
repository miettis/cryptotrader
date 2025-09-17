using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class StcAnalyzer : SkendrAnalyzerBase<StcAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetStc(settings.CyclePeriods, settings.FastPeriods, settings.SlowPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Stc", result.Select(x => x.Stc).ToList() }
            };
        }

        public class Settings
        {
            public int CyclePeriods { get; set; } = 10;
            public int FastPeriods { get; set; } = 23;
            public int SlowPeriods { get; set; } = 50;
        }
    }
}

using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class UltimateAnalyzer : SkendrAnalyzerBase<UltimateAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetUltimate(settings.ShortPeriods, settings.MiddlePeriods, settings.LongPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Ultimate", result.Select(x => x.Ultimate).ToList() }
            };
        }

        public class Settings
        {
            public int ShortPeriods { get; set; } = 7;
            public int MiddlePeriods { get; set; } = 14;
            public int LongPeriods { get; set; } = 28;
        }
    }
}

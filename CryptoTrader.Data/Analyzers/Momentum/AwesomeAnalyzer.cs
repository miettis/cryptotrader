using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class AwesomeAnalyzer : SkendrAnalyzerBase<AwesomeAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetAwesome(settings.FastPeriods, settings.SlowPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Oscillator", result.Select(x => x.Oscillator).ToList() },
                {"Normalized", result.Select(x => x.Normalized).ToList() }
            };
        }

        public class Settings 
        {
            public int FastPeriods { get; set; } = 5;
            public int SlowPeriods { get; set; } = 34;
        }
    }
}

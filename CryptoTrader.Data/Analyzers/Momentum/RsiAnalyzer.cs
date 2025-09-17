using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class RsiAnalyzer : SkendrAnalyzerBase<RsiAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetRsi(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Rsi", result.Select(x => x.Rsi).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 14;
        }
    }
}

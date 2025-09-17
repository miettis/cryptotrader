using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class SmaAnalyzer : SkendrAnalyzerBase<SmaAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetSma(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Sma", result.Select(x => x.Sma).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 14;
        }
    }
}

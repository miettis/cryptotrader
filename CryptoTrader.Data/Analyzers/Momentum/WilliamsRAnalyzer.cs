using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class WilliamsRAnalyzer : SkendrAnalyzerBase<WilliamsRAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetWilliamsR(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"WilliamsR", result.Select(x => x.WilliamsR).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 14;
        }
    }
}

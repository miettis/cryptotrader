using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class HurstAnalyzer : SkendrAnalyzerBase<HurstAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetHurst(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"HurstExponent", result.Select(x => x.HurstExponent).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 100;
        }
    }
}

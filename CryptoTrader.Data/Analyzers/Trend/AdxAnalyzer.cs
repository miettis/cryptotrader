using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class AdxAnalyzer : SkendrAnalyzerBase<AdxAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetAdx(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Adx", result.Select(x => x.Adx).ToList() },
                {"Adxr", result.Select(x => x.Adxr).ToList() },
                {"Mdi", result.Select(x => x.Mdi).ToList() },
                {"Pdi", result.Select(x => x.Pdi).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 14;
        }
    }
}

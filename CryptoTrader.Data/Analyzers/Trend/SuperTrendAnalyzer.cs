using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class SuperTrendAnalyzer : SkendrAnalyzerBase<SuperTrendAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetSuperTrend(settings.LookbackPeriods, settings.Multiplier);
            return new Dictionary<string, List<double?>>
            {
                {"SuperTrend", result.Select(x => (double?)x.SuperTrend).ToList() },
                {"UpperBand", result.Select(x => (double?)x.UpperBand).ToList() },
                {"LowerBand", result.Select(x => (double?)x.LowerBand).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 10;
            public double Multiplier { get; set; } = 3.0;
        }
    }
}

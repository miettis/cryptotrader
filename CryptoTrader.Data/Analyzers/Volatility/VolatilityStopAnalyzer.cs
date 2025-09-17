using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class VolatilityStopAnalyzer : SkendrAnalyzerBase<VolatilityStopAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetVolatilityStop(settings.LookbackPeriods, settings.Multiplier);
            return new Dictionary<string, List<double?>>
            {
                {"Sar", result.Select(x => x.Sar).ToList() },
                {"UpperBand", result.Select(x => x.UpperBand).ToList() },
                {"LowerBand", result.Select(x => x.LowerBand).ToList() },
                // TODO: map boolean to double?
                //{"IsStop", result.Select(x => x.IsStop).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 7;
            public double Multiplier { get; set; } = 3.0;
        }
    }
}

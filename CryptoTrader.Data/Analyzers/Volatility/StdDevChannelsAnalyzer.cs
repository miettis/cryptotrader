using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class StdDevChannelsAnalyzer : SkendrAnalyzerBase<StdDevChannelsAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetStdDevChannels(settings.LookbackPeriods, settings.StandardDeviations);
            return new Dictionary<string, List<double?>>
            {
                {"UpperChannel", result.Select(x => x.UpperChannel).ToList() },
                {"Centerline", result.Select(x => x.Centerline).ToList() },
                {"LowerChannel", result.Select(x => x.LowerChannel).ToList() },
                // TODO: map boolean to double?
                //{"BreakPoint", result.Select(x => x.BreakPoint).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 20;
            public double StandardDeviations { get; set; } = 2.0;
        }
    }
}

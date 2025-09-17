using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class DonchianAnalyzer : SkendrAnalyzerBase<DonchianAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetDonchian(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"UpperBand", result.Select(x => (double?)x.UpperBand).ToList() },
                {"LowerBand", result.Select(x => (double?)x.LowerBand).ToList() },
                {"Centerline", result.Select(x => (double?)x.Centerline).ToList() },
                {"Width", result.Select(x => (double?)x.Width).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 20;
        }
    }
}

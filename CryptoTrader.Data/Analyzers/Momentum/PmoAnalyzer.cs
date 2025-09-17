using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class PmoAnalyzer : SkendrAnalyzerBase<PmoAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetPmo(settings.TimePeriods, settings.SmoothPeriods, settings.SignalPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Pmo", result.Select(x => x.Pmo).ToList() },
                {"Signal", result.Select(x => x.Signal).ToList() }
            };
        }

        public class Settings
        {
            public int TimePeriods { get; set; } = 35;
            public int SmoothPeriods { get; set; } = 20;
            public int SignalPeriods { get; set; } = 10;
        }
    }
}

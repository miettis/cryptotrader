using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class StochAnalyzer : SkendrAnalyzerBase<StochAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetStoch(settings.LookbackPeriods, settings.SignalPeriods, settings.SmoothPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"K", result.Select(x => x.K).ToList() },
                {"D", result.Select(x => x.D).ToList() },
                {"J", result.Select(x => x.J).ToList() },
                {"Oscillator", result.Select(x => x.Oscillator).ToList() },
                {"PercentJ", result.Select(x => x.PercentJ).ToList() },
                {"Signal", result.Select(x => x.Signal).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 14;
            public int SignalPeriods { get; set; } = 3;
            public int SmoothPeriods { get; set; } = 3;
        }
    }
}

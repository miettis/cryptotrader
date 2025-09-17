using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class SmiAnalyzer : SkendrAnalyzerBase<SmiAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetSmi(settings.LookbackPeriods, settings.FirstSmoothPeriods, settings.SecondSmoothPeriods, settings.SignalPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Smi", result.Select(x => x.Smi).ToList() },
                {"Signal", result.Select(x => x.Signal).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 13;
            public int FirstSmoothPeriods { get; set; } = 25;
            public int SecondSmoothPeriods { get; set; } = 2;
            public int SignalPeriods { get; set; } = 3;
        }
    }
}

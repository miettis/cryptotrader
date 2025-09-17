using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class AroonAnalyzer : SkendrAnalyzerBase<AroonAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetAroon(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"AroonUp", result.Select(x => x.AroonUp).ToList() },
                {"AroonDown", result.Select(x => x.AroonDown).ToList() },
                {"Oscillator", result.Select(x => x.Oscillator).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 25;
        }
    }
}

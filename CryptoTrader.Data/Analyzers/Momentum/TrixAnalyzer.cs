using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class TrixAnalyzer : SkendrAnalyzerBase<TrixAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetTrix(settings.LookbackPeriods, settings.SignalPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Trix", result.Select(x => x.Trix).ToList() },
                {"Signal", result.Select(x => x.Signal).ToList() },
                //{"Ema3", result.Select(x => x.Ema3).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 15;
            public int? SignalPeriods { get; set; }
        }
    }
}

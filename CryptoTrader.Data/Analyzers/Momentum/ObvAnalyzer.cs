using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class ObvAnalyzer : SkendrAnalyzerBase<ObvAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetObv(settings.SmaPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Obv", result.Select(x => (double?)x.Obv).ToList() },
                //{"ObvSma", result.Select(x => (double?)x.ObvSma).ToList() }
            };
        }

        public class Settings 
        {
            public int? SmaPeriods { get; set; }
        }
    }
}

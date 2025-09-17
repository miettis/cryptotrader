using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class AdlAnalyzer : SkendrAnalyzerBase<AdlAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetAdl(settings.SmaPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Adl", result.Select(x => (double?)x.Adl).ToList() },
                {"AdlSma", result.Select(x => (double?)x.AdlSma).ToList() },
                {"MoneyFlowMultiplier", result.Select(x => (double?)x.MoneyFlowMultiplier).ToList() },
                {"MoneyFlowVolume", result.Select(x => (double?)x.MoneyFlowVolume).ToList() }
            };
        }

        public class Settings 
        {
            public int? SmaPeriods { get; set; }
        }
    }
}

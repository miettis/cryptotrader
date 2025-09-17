using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class CmfAnalyzer : SkendrAnalyzerBase<CmfAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetCmf(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Cmf", result.Select(x => x.Cmf).ToList() },
                {"MoneyFlowVolume", result.Select(x => x.MoneyFlowVolume).ToList() },
                {"MoneyFlowMultiplier", result.Select(x => x.MoneyFlowMultiplier).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 20;
        }
    }
}

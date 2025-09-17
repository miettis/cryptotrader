using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class TemaAnalyzer : SkendrAnalyzerBase<TemaAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetTema(settings.LookbackPeriods);
            return new Dictionary<string, List<double?>>
            {
                {"Tema", result.Select(x => x.Tema).ToList() }
            };
        }

        public class Settings
        {
            public int LookbackPeriods { get; set; } = 20;
        }
    }
}

using Skender.Stock.Indicators;

namespace CryptoTrader.Data.Analyzers
{
    public class GatorAnalyzer : SkendrAnalyzerBase<GatorAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var result = prices.GetGator();
            return new Dictionary<string, List<double?>>
            {
                {"Upper", result.Select(x => x.Upper).ToList() },
                {"Lower", result.Select(x => x.Lower).ToList() },
                // TODO: map boolean to double?
                //{"UpperIsExpanding", result.Select(x => x.UpperIsExpanding).ToList() },
                //{"LowerIsExpanding", result.Select(x => x.LowerIsExpanding).ToList() },
            };
        }

        public class Settings { }
    }
}

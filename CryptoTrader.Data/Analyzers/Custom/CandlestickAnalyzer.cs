using CryptoTrader.Data.Features.CandleSticks;

namespace CryptoTrader.Data.Analyzers.Custom
{
    public class CandlestickAnalyzer : SecondOrderAnalyzerBase<CandlestickAnalyzer.Settings>
    {
        public CandlestickAnalyzer(BinanceContext context) : base(context)
        {
        }
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var cryptoId = prices.First().CryptoId;
            var stats = Context.CryptoStatistics.FirstOrDefault(x => x.CryptoId == CryptoStatistics.StatsCryptoId);

            var candlestick = new CandleStick(stats);
            var patterns = candlestick.GetAllPatterns(prices);

            var result = new Dictionary<string, List<double?>>();
            foreach (var pattern in Enum.GetNames<CandleStickPattern>()) 
            {
                result.Add(pattern, Enumerable.Range(0,prices.Length).Select(x => (double?)0).ToList());
            }

            for(var i = 0; i < patterns.Length; i++)
            {
                if (patterns[i] == null)
                {
                    continue;
                }
                foreach(var match in patterns[i])
                {
                    result[match.ToString()][i] = 1d;
                }
            }

            return result;
        }
        public override string[] GetOutputs()
        {
            return Enum.GetNames<CandleStickPattern>();
        }
        public class Settings
        {
        }
    }
}

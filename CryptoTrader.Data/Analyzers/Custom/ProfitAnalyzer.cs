namespace CryptoTrader.Data.Analyzers.Custom
{
    public class ProfitAnalyzer : CustomAnalyzerBase<ProfitAnalyzer.Settings>
    {
        public override Dictionary<string, List<double?>> Analyze(Price[] prices, Settings settings)
        {
            var profits = new double?[prices.Length];

            for (var i = 0; i < prices.Length; i++)
            {
                var current = prices[i];
                var highestSellPrice = 0m;
                for(var j = 1; j <= settings.WindowPeriods; j++)
                {
                    if(i + j >= prices.Length)
                    {
                        break;
                    }

                    var next = prices[i + j];
                    var sellPrice = (3 * next.High + next.Low) / 4m;
                    if(sellPrice > highestSellPrice)
                    {
                        highestSellPrice = sellPrice;
                    }
                }

                var buyPrice = (3 * current.Low + current.High) / 4;
                var profit = (highestSellPrice - buyPrice) / buyPrice;
                profits[i] = (double)profit;
            }

            return new Dictionary<string, List<double?>>
            {
                { "Profit", profits.ToList() }
            };
        }
        public override string[] GetOutputs()
        {
            return ["Profit"];
        }
        public class Settings
        {
            public int WindowPeriods { get; set; } = 24;
        }
    }
}

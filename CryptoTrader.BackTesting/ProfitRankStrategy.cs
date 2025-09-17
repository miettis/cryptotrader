using CryptoTrader.Data;
using CryptoTrader.Data.Analyzers.Custom;

namespace CryptoTrader.BackTesting
{
    internal class ProfitRankStrategy : Strategy
    {
        private int _rankFeatureId;

        public ProfitRankStrategy(BinanceContext context) : base(context)
        {
            var indicator = GetIndicator<ProfitRankAnalyzer, ProfitRankAnalyzer.Settings>(x => x.WindowPeriods == 24);
            if (indicator != null)
            {
                var rankOutput = indicator.Analyzer.Outputs.First(x => x.Key == "Rank").Id;
                _rankFeatureId = indicator.Features.First(x => x.OutputId == rankOutput).Id;
            }
        }

        public override Order ShouldBuy(Price[] prices, Order currentOrder)
        {
            var latestPrice = prices.Last();
            var rank = latestPrice.Features.FirstOrDefault(x => x.FeatureId == _rankFeatureId);
            if (rank != null && rank.Value <= 3) 
            {
                return new Order { Type = OrderType.Buy, Price = latestPrice.Low * 1.01m };
            }

            return null;
        }

        public override Order ShouldSell(Price[] prices, Order currentOrder)
        {
            var latestPrice = prices.Last();
            var rank = latestPrice.Features.FirstOrDefault(x => x.FeatureId == _rankFeatureId);
            if (rank != null && rank.Value >= 22)
            {
                var sellPrice = Math.Max(latestPrice.High * 0.99m, currentOrder.Price * 1.03m);
                return new Order { Type = OrderType.Sell, Price = sellPrice };
            }

            return null;
        }

        public override void OrderExecuted(Order order)
        {
        }
    }
}

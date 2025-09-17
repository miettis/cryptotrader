using CryptoTrader.Data;
using CryptoTrader.Data.Analyzers.Custom;

namespace CryptoTrader.BackTesting
{
    internal class SmaCrossoverStrategy : Strategy
    {
        private int _crossAboveId;
        private int _crossBelowId;

        public SmaCrossoverStrategy(BinanceContext context) : base(context)
        {
            var indicator = GetIndicator<MovingAverageCrossoverAnalyzer, MovingAverageCrossoverAnalyzer.Settings>(x => x.OutputKey == "Sma");
            if (indicator != null)
            {
                var outputId1 = indicator.Analyzer.Outputs.First(x => x.Key == "CrossAbove").Id;
                _crossAboveId = indicator.Features.First(x => x.OutputId == outputId1).Id;

                var outputId2 = indicator.Analyzer.Outputs.First(x => x.Key == "CrossBelow").Id;
                _crossBelowId = indicator.Features.First(x => x.OutputId == outputId2).Id;
            }
        }

        public override Order ShouldBuy(Price[] prices, Order currentOrder)
        {
            var latestPrice = prices.Last();
            var crossAbove = latestPrice.Features.FirstOrDefault(x => x.FeatureId == _crossAboveId);
            if (crossAbove != null && crossAbove.Value == 1d) 
            {
                return new Order { Type = OrderType.Buy, Price = latestPrice.Low * 1.01m };
            }

            return null;
        }

        public override Order ShouldSell(Price[] prices, Order currentOrder)
        {
            var latestPrice = prices.Last();
            var crossBelow = latestPrice.Features.FirstOrDefault(x => x.FeatureId == _crossBelowId);
            if (crossBelow != null && crossBelow.Value == 1d)
            {
                return new Order { Type = OrderType.Sell, Price = latestPrice.High * 0.99m };
            }

            return null;
        }

        public override void OrderExecuted(Order order)
        {
        }
    }
}

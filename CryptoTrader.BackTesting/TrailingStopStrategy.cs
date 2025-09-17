using CryptoTrader.Data;

namespace CryptoTrader.BackTesting
{
    internal class TrailingStopStrategy : Strategy
    {
        public TrailingStopStrategy(BinanceContext context) : base(context)
        {
        }
        public override Order ShouldBuy(Price[] prices, Order currentOrder)
        {
            if(currentOrder == null)
            {
                var latestPrice = prices.Last();
                var marketPrice = (latestPrice.High + latestPrice.Low) / 2;

                return new Order 
                { 
                    Type = OrderType.TrailingStopBuy, 
                    ActivationPrice = marketPrice * 0.99m,
                    LimitPrice = marketPrice * 0.98m,
                    TrailingDelta = 0.05m
                };
            }

            return null;
        }

        public override Order ShouldSell(Price[] prices, Order currentOrder)
        {
            if (currentOrder == null)
            {
                var latestPrice = prices.Last();
                var marketPrice = (latestPrice.High + latestPrice.Low) / 2;

                return new Order
                {
                    Type = OrderType.TrailingStopSell,
                    ActivationPrice = marketPrice * 1.01m,
                    LimitPrice = marketPrice * 1.02m,
                    TrailingDelta = 0.05m
                };
            }

            return null;
        }

        public override void OrderExecuted(Order order)
        {
        }
    }
}

using CryptoTrader.Data;
using CryptoTrader.Data.Analyzers;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CryptoTrader.BackTesting
{
    public abstract class Strategy
    {
        protected readonly BinanceContext Context;

        protected Strategy(BinanceContext context)
        {
            Context = context;
        }

        public abstract Order ShouldBuy(Price[] prices, Order currentOrder);
        public abstract Order ShouldSell(Price[] prices, Order currentOrder);
        public abstract void OrderExecuted(Order order);

        public SimulationResult Simulate(int cryptoId, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            var transactions = new List<Order>();
            var prices = Context.Prices
                .AsNoTracking()
                .Include(x => x.Features)
                .Where(x => x.CryptoId == cryptoId && startTime <= x.TimeOpen && x.TimeOpen <= endTime)
                .OrderBy(x => x.TimeOpen)
                .ToArray();

            var startCash = 1000m;
            var cash = startCash;
            var assetAmount = 0m;
            Order order = null;
            var trailingPrice = 0m;
            var trailingActive = false;

            void PlaceOrder(Order newOrder)
            {
                order = newOrder;
                Reset();
            }
            void Reset()
            {
                trailingPrice = 0m;
                trailingActive = false;
            }

            for (var i = 0; i < prices.Length; i++)
            {
                var currentPrice = prices[i];

                if(order != null)
                {
                    var marketPrice = (currentPrice.High + currentPrice.Low) / 2;
                    if (order.Type == OrderType.Buy && order.Price >= currentPrice.Low)
                    {
                        assetAmount = cash / order.Price;
                        cash = 0;
                        transactions.Add(order);
                        OrderExecuted(order);
                        order = null;
                    }
                    else if (order.Type == OrderType.Sell && order.Price < currentPrice.High)
                    {
                        cash = assetAmount * order.Price;
                        assetAmount = 0;
                        transactions.Add(order);
                        OrderExecuted(order);
                        order = null;
                    }
                    else if (order.Type == OrderType.TrailingStopBuy)
                    {
                        if(marketPrice <= order.ActivationPrice)
                        {
                            trailingActive = true;
                        }
                        if (trailingActive)
                        {
                            var newTrailingPrice = (1 + order.TrailingDelta) * marketPrice;
                            if (trailingPrice == 0 || trailingPrice > newTrailingPrice)
                            {
                                trailingPrice = newTrailingPrice;
                            }

                            if (marketPrice >= trailingPrice)
                            {
                                var buyPrice = (order.LimitPrice + marketPrice) / 2;
                                assetAmount = cash / buyPrice;
                                cash = 0;
                                transactions.Add(new Order { Type = OrderType.Buy, Amount = assetAmount, Price = buyPrice });
                                OrderExecuted(order);
                                order = null;
                                Reset();
                            }
                        }
                    }
                    else if (order.Type == OrderType.TrailingStopSell)
                    {
                        if (marketPrice >= order.ActivationPrice)
                        {
                            trailingActive = true;
                        }
                        if (trailingActive)
                        {
                            var newTrailingPrice = (1 - order.TrailingDelta) * marketPrice;
                            if (trailingPrice == 0 || trailingPrice < newTrailingPrice)
                            {
                                trailingPrice = newTrailingPrice;
                            }

                            if (marketPrice <= trailingPrice)
                            {
                                var sellPrice = (order.LimitPrice + marketPrice) / 2;
                                cash = assetAmount * sellPrice;
                                assetAmount = 0;
                                transactions.Add(new Order { Type = OrderType.Sell, Amount = assetAmount, Price = sellPrice });
                                OrderExecuted(order);
                                order = null;
                                Reset();
                            }
                        }
                    }
                }
                

                var pricesSoFar = prices[0..(i + 1)];

                if(cash > 0)
                {
                    var newOrder = ShouldBuy(pricesSoFar, order);
                    if (newOrder != null)
                    {
                        PlaceOrder(newOrder);
                    }
                }
                else if(assetAmount > 0m)
                {
                    var newOrder = ShouldSell(pricesSoFar, order);
                    if (newOrder != null)
                    {
                        PlaceOrder(newOrder);
                    }
                }
            }

            var firstPrice = prices.First();
            var lastPrice = prices.Last();
            var assetValue = assetAmount * ((lastPrice.High + lastPrice.Low) / 2);
            var profit = Math.Round((cash + assetValue - startCash) / startCash * 100m, 2);

            var result = new SimulationResult
            {
                StartPrice = firstPrice.Close,
                EndPrice = lastPrice.Close,
                Cash = cash,
                AssetValue = assetAmount * ((lastPrice.High + lastPrice.Low) / 2),
                Profit = (profit - 1m) * 100,
                Orders = transactions,
            };
            result.HoldProfit = Math.Round((result.EndPrice - result.StartPrice) / result.StartPrice * 100m, 2);

            return result;
        }
        protected Indicator[] GetIndicators<TAnalyzer>()
        {
            return Context.Indicators
                .AsNoTracking()
                .Include(x => x.Features)
                .Include(x => x.Analyzer)
                .ThenInclude(x => x.Outputs)
                .Where(x => x.Analyzer.Type == typeof(TAnalyzer).FullName)
                .ToArray();
        }
        protected Indicator GetIndicator<TAnalyzer, TSettings>(Func<TSettings, bool> predicate)
        {
            var indicators = GetIndicators<TAnalyzer>();
            foreach (var indicator in indicators)
            {
                var settings = JsonSerializer.Deserialize<TSettings>(indicator.Parameters, AnalyzerService.JsonOptions);
                if (settings != null && predicate(settings))
                {
                    return indicator;
                }
            }

            return null;
        }
        
    }
    public class Order
    {
        public OrderType Type { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public decimal ActivationPrice { get; set; }
        public decimal TrailingDelta { get; set; }
        public decimal LimitPrice { get; set; }
    }
    public enum OrderType
    {
        Buy,
        Sell,
        TrailingStopBuy, 
        TrailingStopSell,
    }
    public class SimulationResult
    {
        public decimal Cash { get; set; }
        public decimal AssetValue { get; set; }
        public decimal Profit { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public decimal StartPrice { get; set; }
        public decimal EndPrice { get; set; }
        public decimal HoldProfit { get; set; }
    }
}

using CryptoTrader.Data;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrader.Web.Services
{
    public class OrderRelationUpdateService : CronService
    {
        private readonly IDbContextFactory<BinanceContext> _contextFactory;
        private readonly ILogger<OrderRelationUpdateService> _logger;
        private DateTimeOffset _latestUpdate = DateTimeOffset.MinValue;

        public OrderRelationUpdateService( IDbContextFactory<BinanceContext> contextFactory, ILogger<OrderRelationUpdateService> logger) :
            base("55 * * * * *", TimeZoneInfo.Utc, logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            try
            {
                await UpdateOrders();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error updating order relations | {ex.Message}");
            }
        }

        /*
        public async Task UpdateOrders()
        {
            var context = await _contextFactory.CreateDbContextAsync();
            var unmatchedSell = context.Orders.Where(x => x.Side == OrderSide.Sell && x.Status == OrderStatus.Filled && x.UnmatchedQuantity > 0m).OrderBy(x => x.Created).FirstOrDefault();
            if(unmatchedSell == null)
            {
                return;
            }

            var orders = context.Orders.Include(x => x.SellOrders).Where(x => x.Status == OrderStatus.Filled && x.UnmatchedQuantity > 0m).ToList();

            foreach (var g in orders.GroupBy(x => x.Symbol))
            {
                var symbolOrders = g.OrderBy(x => x.Created).ToList();
                foreach (var sell in symbolOrders.Where(x => x.Side == OrderSide.Sell && x.UnmatchedQuantity > 0m))
                {
                    var buys = symbolOrders.Where(x => x.Side == OrderSide.Buy && x.Created < sell.Created && x.UnmatchedQuantity > 0m)
                        .OrderBy(x => x.Created)
                        .ToList();

                    foreach (var buy in buys)
                    {
                        if(buy.SellOrders.Any(x => x.SellOrderId == sell.Id))
                        {
                            continue;
                        }

                        var quantityToUse = Math.Min(buy.UnmatchedQuantity.Value, sell.UnmatchedQuantity.Value);
                        context.OrderRelations.Add(new OrderRelation
                        {
                            BuyOrder = buy,
                            SellOrder = sell,
                            Quantity = quantityToUse,
                        });

                        buy.UnmatchedQuantity = buy.UnmatchedQuantity.Value - quantityToUse;
                        sell.UnmatchedQuantity = sell.UnmatchedQuantity.Value - quantityToUse;
                        if (sell.UnmatchedQuantity == 0m)
                        {
                            break;
                        }
                    }

                }
            }

            context.SaveChanges();

            _logger.LogInformation($"Updated order relations");
        }
        */
        public async Task UpdateOrders()
        {
            var context = await _contextFactory.CreateDbContextAsync();
            var latestSellUpdate = context.Orders.Where(x => x.Status == OrderStatus.Filled && x.Side == OrderSide.Sell).Max(x => x.Updated);
            if(latestSellUpdate == null || latestSellUpdate <= _latestUpdate)
            {
                return;
            }

            _latestUpdate = latestSellUpdate.Value;

            var orders = context.Orders.Where(x => x.Status == OrderStatus.Filled).ToList();
            await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE order_relation;");
            foreach(var order in orders)
            {
                if(order.Side == OrderSide.Buy)
                {
                    order.UnmatchedQuantity = 0.999m * order.ExecutedQuantity.Value;
                }
                if(order.Side == OrderSide.Sell)
                {
                    order.UnmatchedQuantity = order.ExecutedQuantity.Value;
                }
            }

            foreach (var g in orders.GroupBy(x => x.Symbol))
            {
                var symbolOrders = g.OrderBy(x => x.Created).ToList();
                foreach (var sell in symbolOrders.Where(x => x.Side == OrderSide.Sell && x.UnmatchedQuantity > 0m))
                {
                    var buys = symbolOrders.Where(x => x.Side == OrderSide.Buy && x.Created < sell.Created && x.UnmatchedQuantity > 0m)
                        .OrderBy(x => x.Created)
                        .ToList();

                    foreach (var buy in buys)
                    {
                        var quantityToUse = Math.Min(buy.UnmatchedQuantity.Value, sell.UnmatchedQuantity.Value);
                        context.OrderRelations.Add(new OrderRelation
                        {
                            BuyOrder = buy,
                            SellOrder = sell,
                            Quantity = quantityToUse,
                        });

                        buy.UnmatchedQuantity = buy.UnmatchedQuantity.Value - quantityToUse;
                        sell.UnmatchedQuantity = sell.UnmatchedQuantity.Value - quantityToUse;
                        if (sell.UnmatchedQuantity == 0m)
                        {
                            break;
                        }
                    }
                }
            }

            context.SaveChanges();
        }
    }
}

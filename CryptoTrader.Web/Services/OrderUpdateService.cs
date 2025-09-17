using Binance.Net.Interfaces.Clients;
using Binance.Net.Objects.Models.Spot;
using CryptoTrader.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CryptoTrader.Web.Services
{
    public class OrderUpdateService : CronService
    {
        private readonly IBinanceRestClient _binanceRestClient;
        private readonly IDbContextFactory<BinanceContext> _contextFactory;
        private readonly ILogger<OrderUpdateService> _logger;
        private readonly TradingService _tradingService;
        private DateTimeOffset _latestDailyUpdate = DateTimeOffset.MinValue;

        public OrderUpdateService(IBinanceRestClient binanceRestClient,IDbContextFactory<BinanceContext> contextFactory, ILogger<OrderUpdateService> logger, TradingService tradingService) :
            base("30 */15 * * * *", TimeZoneInfo.Utc, logger)
        {
            _binanceRestClient = binanceRestClient;
            _contextFactory = contextFactory;
            _logger = logger;
            _tradingService = tradingService;
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            if(_latestDailyUpdate < DateTimeOffset.UtcNow.AddHours(-1))
            {
                var startTime = DateTimeOffset.UtcNow.AddDays(-1);
                await UpdateOrders(startTime);
                _latestDailyUpdate = DateTimeOffset.UtcNow;
            }
            else
            {
                await UpdateOpenOrders();
            }
        }

        public async Task UpdateOrders(DateTimeOffset startTime)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            var symbols = context.Orders.Select(x => x.Symbol).Distinct().ToList();
            foreach (var symbol in symbols)
            {
                var result = await _binanceRestClient.SpotApi.Trading.GetOrdersAsync(symbol, startTime: startTime.DateTime);
                if (result.Success)
                {
                    await UpdateOrderDetails(result.Data);
                }
            }

            _logger.LogInformation($"Updated orders");
        }
        public async Task UpdateOpenOrders()
        {
            var statuses = new[] { OrderStatus.New, OrderStatus.PartiallyFilled, OrderStatus.PendingCancel };
            var context = await _contextFactory.CreateDbContextAsync();
            var orders = context.Orders.Where(x => statuses.Contains(x.Status)).GroupBy(x => x.Symbol).Select(x => new { Symbol = x.Key, OldestTime = x.Min(o => o.Created) }).ToList();
            foreach (var order in orders)
            {
                var result = await _binanceRestClient.SpotApi.Trading.GetOrdersAsync(order.Symbol, startTime: order.OldestTime.DateTime);
                if (result.Success)
                {
                    await UpdateOrderDetails(result.Data);
                }
            }

        }

        private async Task UpdateOrderDetails(IEnumerable<BinanceOrder> result)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            foreach (var order in result)
            {
                var dbOrder = context.Orders.FirstOrDefault(x => x.BinanceId == order.Id);
                if (dbOrder == null)
                {
                    dbOrder = new Order
                    {
                        BinanceId = order.Id,
                        Symbol = order.Symbol,
                        Side = (OrderSide)order.Side,
                        Quantity = order.Quantity,
                        Price = order.Price,
                        Status = (OrderStatus)order.Status,
                        Created = order.CreateTime,
                        Updated = order.UpdateTime ?? order.CreateTime,
                        ExecutedQuantity = order.QuantityFilled,
                        QuoteQuantity = order.QuoteQuantityFilled,
                        AverageFillPrice = order.AverageFillPrice,
                        CreateResponse = JsonConvert.SerializeObject(order)
                    };
                    context.Orders.Add(dbOrder);
                }
                else
                {
                    var orderFilled = order.Status == Binance.Net.Enums.OrderStatus.Filled && dbOrder.Status != OrderStatus.Filled;
                    dbOrder.Status = (OrderStatus)order.Status;
                    dbOrder.ExecutedQuantity = order.QuantityFilled;
                    dbOrder.QuoteQuantity = order.QuoteQuantityFilled;
                    dbOrder.AverageFillPrice = order.AverageFillPrice;
                    dbOrder.CreateResponse = JsonConvert.SerializeObject(order);
                    dbOrder.Updated = order.UpdateTime ?? dbOrder.Updated ?? order.CreateTime;
                    
                    if (orderFilled && order.Side == Binance.Net.Enums.OrderSide.Buy)
                    {
                        try
                        {
                            await _tradingService.SellDefault(dbOrder);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error making default sell order {dbOrder.Symbol} | {ex.Message}");
                        }
                    }
                }
                if (dbOrder.Status == OrderStatus.Filled)
                {
                    if (dbOrder.Side == OrderSide.Buy && dbOrder.Commission == null) 
                    { 
                        dbOrder.Commission = 0.001m * dbOrder.ExecutedQuantity;
                    }
                    if (dbOrder.Side == OrderSide.Buy && dbOrder.UnmatchedQuantity == null)
                    {
                        dbOrder.UnmatchedQuantity = dbOrder.ExecutedQuantity.Value - (dbOrder.Commission ?? 0m);
                    }
                    if (dbOrder.Side == OrderSide.Sell && dbOrder.UnmatchedQuantity == null)
                    {
                        dbOrder.UnmatchedQuantity = dbOrder.ExecutedQuantity;
                    }
                }
                
            }

            await context.SaveChangesAsync();
        }
    }
}

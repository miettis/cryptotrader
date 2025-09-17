using Binance.Net.Interfaces.Clients;
using Binance.Net.Objects.Models.Spot;
using CryptoTrader.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CryptoTrader.Web.Services
{
    public class OpenOrderService : CronService
    {
        public BinanceOrder[]? OpenOrders
        {
            get
            {
                return _orders;
            }
            private set
            {
                _orders = value;
                Updated = DateTimeOffset.UtcNow;
            }
        }
        public DateTimeOffset? Updated { get; private set; }

        private BinanceOrder[]? _orders;
        private readonly IBinanceRestClient _binanceRestClient;
        private readonly IDbContextFactory<BinanceContext> _contextFactory;
        private readonly ILogger<OpenOrderService> _logger;

        public OpenOrderService(IBinanceRestClient binanceRestClient, IDbContextFactory<BinanceContext> contextFactory, ILogger<OpenOrderService> logger) :
            base("0 15 * * * *", TimeZoneInfo.Utc, logger)
        {
            _binanceRestClient = binanceRestClient;
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            var orders = new List<BinanceOrder>();       
            var result = await _binanceRestClient.SpotApi.Trading.GetOpenOrdersAsync();
            if (result.Success)
            {
                orders.AddRange(result.Data);

                var ids = result.Data.Select(x => x.Id);
                
                var dbOrders = context.Orders.Where(x => ids.Contains(x.Id)).ToList();
                foreach(var order in result.Data)
                {
                    var dbOrder = dbOrders.FirstOrDefault(x => x.Id == order.Id);
                    if(dbOrder != null)
                    {
                        dbOrder.Status = (OrderStatus)order.Status;
                    }
                }
                await context.SaveChangesAsync();
            }
            _logger.LogInformation($"Updated open orders");
        }

        public async Task RefreshIfOlderThan(TimeSpan timeSpan)
        {
            if (Updated == null || Updated.Value.Add(timeSpan) < DateTimeOffset.UtcNow)
            {
                await DoWork(CancellationToken.None);
            }
        }
    }
}

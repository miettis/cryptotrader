using Binance.Net.Enums;
using FastEndpoints;

namespace CryptoTrader.Web.Events
{
    public class OrderCreatedEvent : IEvent
    {
        public string Symbol { get; set; }
        public OrderSide Side { get; set; }
        public long BinanceId { get; set; }
    }

    public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;

        public OrderCreatedEventHandler(IServiceScopeFactory scopeFactory, ILogger<OrderCreatedEvent> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task HandleAsync(OrderCreatedEvent evt, CancellationToken ct)
        {
       
        }
    }
}

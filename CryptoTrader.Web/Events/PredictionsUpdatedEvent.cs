using CryptoTrader.Data;
using CryptoTrader.Web.Services;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace CryptoTrader.Web.Events
{
    public class PredictionsUpdatedEvent : IEvent
    {
        public string Symbol { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
    }

    public class PredictionsUpdatedEventHandler : IEventHandler<PredictionsUpdatedEvent>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;

        public PredictionsUpdatedEventHandler(IServiceScopeFactory scopeFactory, ILogger<PredictionsUpdatedEvent> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task HandleAsync(PredictionsUpdatedEvent evt, CancellationToken ct)
        {
            if (evt.Start.IsCurrentHour()) 
            {
                using var scope = _scopeFactory.CreateScope();
                var contextFactory = scope.Resolve<IDbContextFactory<BinanceContext>>();
                var context = await contextFactory.CreateDbContextAsync();
                var tradingService = scope.Resolve<TradingService>();
                var crypto = context.Cryptos.Include(x => x.Models).AsNoTracking().First(x => x.Symbol == evt.Symbol);

                await tradingService.CheckTradingOpportunities(crypto);
            }
        }
    }
}

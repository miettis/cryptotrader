using CryptoTrader.Data;
using CryptoTrader.Web.Services;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrader.Web.Events
{
    public class FeaturesUpdatedEvent : IEvent
    {
        public string Symbol { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
    }

    public class FeaturesUpdatedEventHandler : IEventHandler<FeaturesUpdatedEvent>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;

        public FeaturesUpdatedEventHandler(IServiceScopeFactory scopeFactory, ILogger<FeaturesUpdatedEvent> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task HandleAsync(FeaturesUpdatedEvent evt, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var contextFactory = scope.Resolve<IDbContextFactory<BinanceContext>>();
            var context = await contextFactory.CreateDbContextAsync();
            var predictionService = scope.Resolve<PredictionService>();
            var crypto = context.Cryptos.Include(x => x.Models).AsNoTracking().First(x => x.Symbol == evt.Symbol);

            await predictionService.UpdatePredictions(crypto);
        }
    }
}

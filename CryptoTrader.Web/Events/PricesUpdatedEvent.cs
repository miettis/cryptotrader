using CryptoTrader.Web.Services;
using FastEndpoints;

namespace CryptoTrader.Web.Events
{
    public class PricesUpdatedEvent: IEvent
    {
        public string Symbol { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
    }

    public class PricesUpdatedEventHandler : IEventHandler<PricesUpdatedEvent>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;

        public PricesUpdatedEventHandler(IServiceScopeFactory scopeFactory, ILogger<PricesUpdatedEvent> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task HandleAsync(PricesUpdatedEvent evt, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var featureCalculationService = scope.Resolve<FeatureCalculationService>();

            await featureCalculationService.CalculateFeatures(evt.Symbol);
        }
    }
}

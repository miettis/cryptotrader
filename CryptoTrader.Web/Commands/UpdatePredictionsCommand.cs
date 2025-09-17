using CryptoTrader.Data;
using CryptoTrader.Web.Services;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrader.Web.Commands
{
    public class UpdatePredictionsCommand : ICommand
    {
        public string Symbol { get; set; }
    }
    public class UpdatePredictionsCommandHandler : ICommandHandler<UpdatePredictionsCommand>
    {
        private readonly IDbContextFactory<BinanceContext> _contextFactory;
        private readonly PredictionService _predictionService;

        public UpdatePredictionsCommandHandler(IDbContextFactory<BinanceContext> contextFactory, PredictionService predictionService)
        {
            _contextFactory = contextFactory;
            _predictionService = predictionService;
        }

        public async Task ExecuteAsync(UpdatePredictionsCommand command, CancellationToken ct)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            var crypto = context.Cryptos.Include(x => x.Models).AsNoTracking().First(x => x.Symbol == command.Symbol);

            await _predictionService.UpdatePredictions(crypto);
        }
    }
}

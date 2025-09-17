using CryptoTrader.Web.Services;
using FastEndpoints;

namespace CryptoTrader.Web.Commands
{
    public class UpdateFeaturesCommand : ICommand
    {
        public string Symbol { get; set; }
    }
    public class UpdateFeaturesCommandHandler : ICommandHandler<UpdateFeaturesCommand>
    {
        private readonly FeatureCalculationService _featureCalculationService;
        public UpdateFeaturesCommandHandler(FeatureCalculationService featureCalculationService)
        {
            _featureCalculationService = featureCalculationService;
        }
        public async Task ExecuteAsync(UpdateFeaturesCommand command, CancellationToken ct)
        {
            await _featureCalculationService.CalculateFeatures(command.Symbol);
        }
    }
}

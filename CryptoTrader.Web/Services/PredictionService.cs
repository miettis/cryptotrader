using Binance.Net.Enums;
using Binance.Net.Interfaces.Clients;
using CryptoTrader.Data;
using CryptoTrader.Data.Extensions;
using CryptoTrader.Data.Services;
using CryptoTrader.Web.Events;
using CryptoTrader.Web.Utils;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrader.Web.Services
{
    public class PredictionService : CronService
    {
        private readonly IBinanceRestClient _binanceRestClient;
        private readonly IDbContextFactory<BinanceContext> _contextFactory;
        private readonly ILogger<PredictionService> _logger;
        private readonly FeatureCalculationService _featureCalculationService;
        private readonly AccountInfoService _accountInfoService;
        private readonly PythonService _pythonService;
        private DateTimeOffset _latestUpdate = DateTimeOffset.MinValue;
        private bool _running = false;
        private BinanceImportConfig _importConfig = new BinanceImportConfig
        {
            DailyDirectory = "daily",
            MonthlyDirectory = "monthly",
            DeleteFileAfterInsert = true,
            SleepAfterFetch = 5000,
            UpdateDataStartTime = false
        };

        public PredictionService(IDbContextFactory<BinanceContext> contextFactory, ILogger<PredictionService> logger, PythonService pythonService, AccountInfoService accountInfoService) : 
            base("0 15 * * * *", TimeZoneInfo.Utc, logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
            _pythonService = pythonService;
            _accountInfoService = accountInfoService;
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            if (_running || _latestUpdate > DateTimeOffset.UtcNow.StartOfHour())
            {
                return;
            }

            _running = true;

            var ownedSymbols = (await _accountInfoService.GetOwnedSymbols()).Select(x => x.AsSymbolPair()).ToArray();
            var context = _contextFactory.CreateDbContext(); 
            var cryptos = context.Cryptos.Include(x => x.Models).Where(x => x.Followed.HasValue).AsNoTracking().ToList();

            foreach (var crypto in cryptos)
            {
                try
                {
                    await UpdatePredictions(crypto);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed updating predictions {crypto.Symbol} | {ex.Message}");
                }
            }

            _latestUpdate = context.Predictions.Max(x => x.Created);
            _running = false;
        }
        public async Task UpdatePredictions(Crypto crypto)
        {
            var context = _contextFactory.CreateDbContext();
            var latestPrediction = await context.Predictions.Where(x => x.CryptoId == crypto.Id).OrderByDescending(x => x.TimeOpen).FirstOrDefaultAsync();
            if(latestPrediction != null && latestPrediction.TimeOpen == DateTimeOffset.UtcNow.StartOfHour())
            {
                return;
            }

            var time = DateTimeOffset.UtcNow.StartOfHour().AddHours(-1);
            var models = crypto.Models.GroupBy(x => x.Output).Select(x => x.OrderByDescending(x => x.Created).First()).ToList();
            var predictions = await _pythonService.Predict(crypto.Id, time, models);
            var prediction = new PricePrediction
            {
                CryptoId = crypto.Id,
                TimeOpen = time.AddHours(1),
                Created = DateTimeOffset.UtcNow
            };
            foreach (var kvp in predictions.Predictions)
            {
                var parts = kvp.Key.Split('_');
                if (kvp.Key.StartsWith("price_return", StringComparison.OrdinalIgnoreCase))
                {
                    var intervalName = parts[2];
                    var rank = parts[3];

                    var intervalProperty = typeof(PricePrediction).GetProperties().FirstOrDefault(x => x.Name.Equals(intervalName, StringComparison.OrdinalIgnoreCase));
                    var rankProperty = typeof(PredictionRank).GetProperties().FirstOrDefault(x => x.Name.Equals(rank, StringComparison.OrdinalIgnoreCase));

                    var intervalRanks = intervalProperty.GetValue(prediction);
                    rankProperty.SetValue(intervalRanks, (int)kvp.Value);
                }
                else
                {
                    var propertyName = parts[1];
                    var property = typeof(PricePrediction).GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
                    property.SetValue(prediction, kvp.Value);
                }
            }
            await context.Predictions.AddAsync(prediction);
            await context.SaveChangesAsync();

            _logger.LogInformation($"Updated predictions {crypto.Symbol}");

            await new PredictionsUpdatedEvent
            {
                Symbol = crypto.Symbol,
                Start = prediction.TimeOpen,
                End = prediction.TimeOpen.AddHours(1).AddMilliseconds(-1)
            }.PublishAsync(Mode.WaitForNone);
        }
    }
}

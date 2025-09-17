using CryptoTrader.Data;
using CryptoTrader.Data.Features;
using CryptoTrader.Data.Services;
using CryptoTrader.Web.Commands;
using CryptoTrader.Web.Events;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrader.Web.Services
{
    public class FeatureCalculationService : CronService
    {
        private readonly IDbContextFactory<BinanceContext> _contextFactory;
        private readonly ILogger<FeatureCalculationService> _logger;
        private readonly FeatureCalculation _featureCalculation;
        private DateTimeOffset _latestCalculation = DateTimeOffset.MinValue;
        private bool _running = false;
        private Dictionary<int, DateTimeOffset> _latestCryptoUpdates = new Dictionary<int, DateTimeOffset>();

        public FeatureCalculationService( IDbContextFactory<BinanceContext> contextFactory, ILogger<FeatureCalculationService> logger, FeatureCalculation featureCalculation) : 
            base("0 10 * * * *", TimeZoneInfo.Utc, logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
            _featureCalculation = featureCalculation;
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            if(_running || _latestCalculation > DateTimeOffset.UtcNow.StartOfHour())
            {
                return;
            }

            _running = true;

            var context = _contextFactory.CreateDbContext();
            var cryptos = context.Cryptos.AsNoTracking().Where(x => x.Active && x.Trade || x.Followed.HasValue).ToList();
            foreach(var crypto in cryptos)
            {
                await CalculateFeatures(crypto);
            }

            _latestCalculation = DateTimeOffset.UtcNow;
            _running = false;
        }
        public async Task CalculateFeatures(string symbol)
        {
            var context = _contextFactory.CreateDbContext();
            var crypto = context.Cryptos.AsNoTracking().First(x => x.Symbol == symbol);
            await CalculateFeatures(crypto);
        }
        public async Task CalculateFeatures(Crypto crypto)
        {
            if(_latestCryptoUpdates.TryGetValue(crypto.Id, out var latestUpdate) && latestUpdate > DateTimeOffset.UtcNow.StartOfHour())
            {
                return;
            }

            var endTime = DateTimeOffset.UtcNow.StartOfHour().AddMilliseconds(-1);
            await CalculateMovingAverages(crypto.Symbol, (crypto.Times.EndMA ?? crypto.Times.StartData).AddDays(-2), endTime);
            await CalculateTrends(crypto.Symbol, (crypto.Times.EndSlope ?? crypto.Times.StartData).AddDays(-2), endTime);
            await CalculateSlopes(crypto.Symbol, (crypto.Times.EndSlope ?? crypto.Times.StartData).AddDays(-2), endTime);
            await CalculateReturns(crypto.Symbol, (crypto.Times.EndReturn ?? crypto.Times.StartData).AddDays(-2), endTime);
            await CalculateCycles(crypto.Symbol, (crypto.Times.EndCycle ?? crypto.Times.StartData).AddDays(-2), endTime);
            await CalculateMiscFeatures(crypto.Symbol, (crypto.Times.EndOther ?? crypto.Times.StartData).AddDays(-2), endTime);
            await CalculateMomentum(crypto.Symbol, (crypto.Times.EndMomentum ?? crypto.Times.StartData).AddDays(-2), endTime);
            await CalculatePeaks(crypto.Symbol, (crypto.Times.EndPeak ?? crypto.Times.StartData).AddDays(-2), endTime);
            await CalculateVolatility(crypto.Symbol, (crypto.Times.EndVolatility ?? crypto.Times.StartData).AddDays(-2), endTime);
            await CalculateVolume(crypto.Symbol, (crypto.Times.EndVolume ?? crypto.Times.StartData).AddDays(-2), endTime);
            await CalculateCandlesticks(crypto.Symbol, (crypto.Times.EndCandleSticks ?? crypto.Times.StartData).AddDays(-2), endTime);

            _latestCryptoUpdates[crypto.Id] = DateTimeOffset.UtcNow;

            await new FeaturesUpdatedEvent
            {
                Symbol = crypto.Symbol,
                End = endTime
            }.PublishAsync(Mode.WaitForNone);
        }
        public async Task CalculateCandlesticks(string symbol, DateTimeOffset start, DateTimeOffset end)
        {
            try
            {
                _featureCalculation.UpdateCandleSticks(symbol, start.GetDate(), end.GetDate());
                _logger.LogInformation($"Updated candlesticks {symbol} {start} - {end}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed calculating candlesticks {symbol} {start} - {end} | {ex.Message}");
            }
        }
        public async Task CalculateCycles(string symbol, DateTimeOffset start, DateTimeOffset end)
        {
            try
            {
                _featureCalculation.UpdateCycles(symbol, start.GetDate(), end.GetDate());
                _logger.LogInformation($"Updated cycles {symbol} {start} - {end}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed calculating cycles {symbol} {start} - {end} | {ex.Message}");
            }
        }
        public async Task CalculateMiscFeatures(string symbol, DateTimeOffset start, DateTimeOffset end)
        {
            try
            {
                _featureCalculation.UpdateOtherIndicators(symbol, start.GetDate(), end.GetDate());
                _logger.LogInformation($"Updated misc features {symbol} {start} - {end}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed calculating misc features {symbol} {start} - {end} | {ex.Message}");
            }
        }
        public async Task CalculateMovingAverages(string symbol, DateTimeOffset start, DateTimeOffset end)
        {
            try
            {
                _featureCalculation.UpdateMovingAverages(symbol, start.GetDate(), end.GetDate());
                _logger.LogInformation($"Updated moving averages {symbol} {start} - {end}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed calculating moving averages {symbol} {start} - {end} | {ex.Message}");
            }
        }       
        public async Task CalculateMomentum(string symbol, DateTimeOffset start, DateTimeOffset end)
        {
            try
            {
                _featureCalculation.UpdateMomentum(symbol, start.GetDate(), end.GetDate());
                _logger.LogInformation($"Updated momentum {symbol} {start} - {end}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed calculating momentum {symbol} {start} - {end} | {ex.Message}");
            }
        }
        public async Task CalculatePeaks(string symbol, DateTimeOffset start, DateTimeOffset end)
        {
            try
            {
                _featureCalculation.UpdatePeaks(symbol, start.GetDate(), end.GetDate());
                _logger.LogInformation($"Updated peaks {symbol} {start} - {end}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed calculating peaks {symbol} {start} - {end} | {ex.Message}");
            }
        }
        public async Task CalculateReturns(string symbol, DateTimeOffset start, DateTimeOffset end)
        {
            try
            {
                _featureCalculation.UpdateReturns(symbol, start.GetDate(), end.GetDate());
                _logger.LogInformation($"Updated returns {symbol} {start} - {end}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed calculating returns {symbol} {start} - {end} | {ex.Message}");
            }
        }
        public async Task CalculateTrends(string symbol, DateTimeOffset start, DateTimeOffset end)
        {       
            try
            {
                _featureCalculation.UpdateTrends(symbol, start.GetDate(), end.GetDate());
                _logger.LogInformation($"Updated trends {symbol} {start} - {end}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed calculating trends {symbol} {start} - {end} | {ex.Message}");
            }
        }
        public async Task CalculateSlopes(string symbol, DateTimeOffset start, DateTimeOffset end)
        {
            try
            {
                _featureCalculation.UpdateSlopes(symbol, start.GetDate(), end.GetDate());
                _logger.LogInformation($"Updated slopes {symbol} {start} - {end}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed calculating slopes {symbol} {start} - {end} | {ex.Message}");
            }
        }
        public async Task CalculateVolatility(string symbol, DateTimeOffset start, DateTimeOffset end)
        {
            try
            {
                _featureCalculation.UpdateVolatilities(symbol, start.GetDate(), end.GetDate());
                _logger.LogInformation($"Updated volatility {symbol} {start} - {end}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed calculating volatility {symbol} {start} - {end} | {ex.Message}");
            }
        }
        public async Task CalculateVolume(string symbol, DateTimeOffset start, DateTimeOffset end)
        {
            try
            {
                _featureCalculation.UpdateVolumes(symbol, start.GetDate(), end.GetDate());
                _logger.LogInformation($"Updated volume {symbol} {start} - {end}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed calculating volume {symbol} {start} - {end} | {ex.Message}");
            }
        }
    }
}

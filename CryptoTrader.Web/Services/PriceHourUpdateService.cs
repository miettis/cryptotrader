using Binance.Net.Enums;
using Binance.Net.Interfaces.Clients;
using CryptoTrader.Data;
using CryptoTrader.Data.Extensions;
using CryptoTrader.Web.Events;
using CryptoTrader.Web.Utils;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrader.Web.Services
{
    public class PriceHourUpdateService : CronService
    {
        private readonly IBinanceRestClient _binanceRestClient;
        private readonly IDbContextFactory<BinanceContext> _contextFactory;
        private readonly ILogger<PriceHourUpdateService> _logger;
        private readonly FeatureCalculationService _featureCalculationService;
        private readonly AccountInfoService _accountInfoService;
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

        public PriceHourUpdateService(IBinanceRestClient binanceRestClient, IDbContextFactory<BinanceContext> contextFactory, ILogger<PriceHourUpdateService> logger, FeatureCalculationService featureCalculationService, AccountInfoService accountInfoService) : 
            base("45 */5 * * * *", TimeZoneInfo.Utc, logger)
        {
            _binanceRestClient = binanceRestClient;
            _contextFactory = contextFactory;
            _logger = logger;
            _featureCalculationService = featureCalculationService;
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
            var cryptos = context.Cryptos.Where(x => x.Active).AsNoTracking().ToList();

            foreach (var crypto in cryptos)
            {
                var prices = context.Prices.Where(x => x.CryptoId == crypto.Id).OrderByDescending(x => x.TimeOpen).Take(1).ToList();
                var startTime = crypto.Times.EndData.HasValue ? crypto.Times.EndData.Value.AddHours(1) : crypto.Times.StartData;
                try
                {
                    var useApi = crypto.Trade || crypto.Followed.HasValue || ownedSymbols.Contains(crypto.Symbol);
                    await UpdatePrices(crypto.Symbol, startTime, useApi);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed updating prices {crypto.Symbol} | {ex.Message}");
                }
            }

            _latestUpdate = DateTimeOffset.UtcNow;
            _running = false;
        }
        public async Task UpdatePrices(string symbol)
        {
            symbol = symbol.AsSymbolPair();

            var context = _contextFactory.CreateDbContext();
            var crypto = await context.Cryptos.FirstAsync(x => x.Symbol == symbol);
            var startTime = crypto.Times.EndData ?? crypto.Times.StartData;
            await UpdatePrices(symbol, startTime);
        }
        public async Task UpdatePrices(string symbol, DateTimeOffset startTime, bool useApi = true)
        {
            var time = startTime;
            symbol = symbol.AsSymbolPair();

            _logger.LogInformation($"Updating {symbol} {time} ->");

            if (time < DateTimeOffset.UtcNow.StartOfMonth().AddMonths(-1))
            {
                time = await ImportPricesFromMonthlyFiles(symbol, time);
            }
            if (time < DateTimeOffset.UtcNow.StartOfDay())
            {
                time = await ImportPricesFromDailyFiles(symbol, time);
            }

            var context = _contextFactory.CreateDbContext();

            if (useApi) 
            { 
                var weekAgo = DateTimeOffset.UtcNow.StartOfHour().AddHours(-7 * 24); 
                var cryptoTimestamps = context.Prices
                    .Where(x => x.Crypto.Symbol == symbol && x.TimeOpen >= weekAgo)
                    .Select(x => x.TimeOpen)
                    .ToList();

                time = await ImportPricesFromApi(symbol, weekAgo, cryptoTimestamps);
            }
            try
            {
                await context.UpdateCryptoDataEndTime();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed updating data end time | {ex.Message}");
            }

            if(time > startTime)
            {
                await new PricesUpdatedEvent
                {
                    Symbol = symbol,
                    Start = startTime,
                    End = time.AddHours(1).AddMilliseconds(-1)
                }.PublishAsync(Mode.WaitForNone);
            }
        }

        private async Task<DateTimeOffset> ImportPricesFromMonthlyFiles(string symbol, DateTimeOffset start)
        {
            var end = DateTimeOffset.UtcNow.StartOfMonth().AddDays(-1).GetDate();

            _logger.LogInformation($"Importing monthly files {symbol} {start} -> {end}");

            var import = new BinanceImport(_contextFactory, _logger, _importConfig);
            await import.FetchMonthly(symbol, "1h", start.GetDate(), end);
            try
            {
                var latestTimestamp = await import.ImportMonthly("1h", start.GetDate(), end, [symbol]);
                _logger.LogInformation($"Imported monthly files {symbol} {start} -> {latestTimestamp}");
                return latestTimestamp;
            } 
            catch (Exception ex)
            {
                _logger.LogError($"Error importing monthly files {symbol} {start.GetDate()} - {end} | {ex.Message}");
                throw;
            }
        }
        private async Task<DateTimeOffset> ImportPricesFromDailyFiles(string symbol, DateTimeOffset start)
        {
            var end = DateTimeOffset.UtcNow.StartOfDay().AddDays(-1).GetDate();

            _logger.LogInformation($"Importing daily files {symbol} {start} -> {end}");

            var import = new BinanceImport(_contextFactory, _logger, _importConfig);
            await import.FetchDaily(symbol, "1h", start.GetDate(), end);
            try
            {
                var latestTimestamp = await import.ImportDaily("1h", start.GetDate(), end, [symbol]);
                _logger.LogInformation($"Imported daily files {symbol} {start} -> {latestTimestamp}");
                return latestTimestamp;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error importing daily data {symbol} {start.GetDate()} - {end} | {ex.Message}");
                throw;
            }
        }
        private async Task<DateTimeOffset> ImportPricesFromApi(string symbol, DateTimeOffset start, IEnumerable<DateTimeOffset> existing)
        {
            _logger.LogInformation($"Importing from api {symbol} {start} ->");

            var end = DateTimeOffset.UtcNow.StartOfHour().AddMilliseconds(-1);
            var result = await _binanceRestClient.SpotApi.ExchangeData.GetKlinesAsync(symbol, KlineInterval.OneHour, start.DateTime, end.DateTime);
            if (result.Success)
            {
                _logger.LogInformation($"Fetched data {symbol} {result.Data.FirstOrDefault()?.OpenTime} - {result.Data.LastOrDefault()?.CloseTime}");

                var cryptoContext = await _contextFactory.CreateDbContextAsync();
                var crypto = await cryptoContext.Cryptos.FirstAsync(x => x.Symbol == symbol);
                try
                {
                    foreach (var kline in result.Data)
                    {
                        if (existing.Contains(kline.OpenTime))
                        {
                            continue;
                        }

                        var price = kline.ToPrice<Price>();
                        price.CryptoId = crypto.Id;

                        await cryptoContext.Prices.AddAsync(price);
                    }

                    await cryptoContext.SaveChangesAsync();
                    _logger.LogInformation($"Imported api data {symbol} {result.Data.FirstOrDefault()?.OpenTime} - {result.Data.LastOrDefault()?.CloseTime}");

                    return result.Data.Max(x => x.OpenTime);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Error saving api data {symbol} {result.Data.FirstOrDefault()?.OpenTime} - {result.Data.LastOrDefault()?.CloseTime} | {ex.Message}");
                }
            }

            return start;
        }
    }
}

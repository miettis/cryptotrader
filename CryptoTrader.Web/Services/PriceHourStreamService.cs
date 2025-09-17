using Binance.Net.Enums;
using Binance.Net.Interfaces.Clients;
using CryptoExchange.Net.Sockets;
using CryptoTrader.Data;
using CryptoTrader.Data.Extensions;
using CryptoTrader.Web.Events;
using CryptoTrader.Web.Utils;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrader.Web.Services
{
    public class PriceHourStreamService : CronService
    {
        private IBinanceSocketClient _socketClient;
        private UpdateSubscription? _subscription;
        private DateTimeOffset _subscriptionStart;
        private readonly IDbContextFactory<BinanceContext> _contextFactory;
        private readonly ILogger<PriceHourStreamService> _logger;
        private readonly AccountInfoService _accountInfoService;
        private Dictionary<string, Crypto> _cryptoMapping = new Dictionary<string, Crypto>();
        private Dictionary<string, Price> _cryptoBuffer = new Dictionary<string, Price>();

        public PriceHourStreamService(IBinanceSocketClient socketClient, IDbContextFactory<BinanceContext> contextFactory, AccountInfoService accountInfoService, ILogger<PriceHourStreamService> logger) : base("0 * * * * *", TimeZoneInfo.Utc, logger)
        {
            _socketClient = socketClient;
            _contextFactory = contextFactory;
            _accountInfoService = accountInfoService;
            _logger = logger;
        }
        public override async Task DoWork(CancellationToken cancellationToken)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            var cryptos = await context.Cryptos.Where(x => x.Active).AsNoTracking().ToListAsync();
            var configChanged = false;
            foreach (var crypto in cryptos)
            {
                if(_cryptoMapping.TryGetValue(crypto.Symbol, out var existing) && existing.Trade != crypto.Trade)
                {
                    configChanged = true;
                } 
                _cryptoMapping[crypto.Symbol] = crypto;
            }

            if (configChanged || _subscriptionStart < DateTimeOffset.UtcNow.AddHours(-12))
            {
                await RefreshSubscription();
            }
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);

            var context = await _contextFactory.CreateDbContextAsync();
            var cryptos = await context.Cryptos.Where(x => x.Active).AsNoTracking().ToListAsync();
            foreach (var crypto in cryptos)
            {
                _cryptoMapping.TryAdd(crypto.Symbol, crypto);
            }
            await RefreshSubscription();
        }


        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            if (_subscription != null)
            {
                await _socketClient.UnsubscribeAsync(_subscription);
            }
        }

        public Price? GetLatestPrice(string symbol)
        {
            if(_cryptoBuffer.TryGetValue(symbol, out var result))
            {
                return result;
            }
            return null;
        }

        public async Task RefreshSubscription()
        {
            if (_subscription != null)
            {
                await _socketClient.UnsubscribeAsync(_subscription);
            }

            var ownedSymbols = (await _accountInfoService.GetOwnedSymbols()).Select(x => x.AsSymbolPair());
            var context = await _contextFactory.CreateDbContextAsync();
            var symbols = context.Cryptos.Where(x => x.Followed.HasValue).Select(x => x.Symbol).Union(ownedSymbols).Distinct().ToList();
            if(symbols.Count == 0)
            {
                return;
            }

            var subResult = await _socketClient.SpotApi.ExchangeData.SubscribeToKlineUpdatesAsync(symbols, KlineInterval.OneHour, async data =>
            {
                var symbol = data.Data.Symbol;
                var ohlc = data.Data.Data;
                if (_cryptoMapping.TryGetValue(symbol, out var crypto))
                {
                    var price = ohlc.ToPrice<Price>();
                    price.CryptoId = crypto.Id;

                    if (ohlc.Final)
                    {
                        var context = await _contextFactory.CreateDbContextAsync();
                        try
                        {
                            await context.Prices.AddAsync(price);
                            await context.SaveChangesAsync(); 
                            await context.UpdateCryptoDataEndTime(crypto.Id);
                            _logger.LogInformation($"Saved hour price data: {symbol} {price.TimeOpen}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Failed saving hour price data: {symbol} {price.TimeOpen} | {ex.Message}");
                        }

                        _cryptoBuffer[symbol] = price;

                        await new PricesUpdatedEvent
                        {
                            Symbol = symbol,
                            Start = price.TimeOpen,
                            End = price.TimeOpen.AddHours(1).AddMilliseconds(-1)
                        }.PublishAsync(Mode.WaitForNone);
                    }
                    else
                    {
                        price.Crypto = crypto;
                        _cryptoBuffer[symbol] = price;
                    }

                }

            });
            if (subResult.Success)
            {
                _subscription = subResult.Data;
                _subscriptionStart = DateTimeOffset.UtcNow;
                _logger.LogInformation($"Subscribed to symbols {string.Join(", ", symbols)}");
            }
            else
            {
                _logger.LogError($"Failed to subscribe to symbols {string.Join(", ", symbols)}");
            }
        }
    }
}

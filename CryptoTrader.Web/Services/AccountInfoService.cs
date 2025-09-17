using Binance.Net.Interfaces.Clients;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.CommonObjects;
using CryptoTrader.Data;
using CryptoTrader.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrader.Web.Services
{
    public class AccountInfoService : CronService
    {
        public BinanceAccountInfo? Account
        {
            get
            {
                return _account;
            }
            private set
            {
                _account = value;
                Updated = DateTimeOffset.UtcNow;
            }
        }
        public DateTimeOffset Updated { get; private set; } = DateTimeOffset.MinValue;

        private BinanceAccountInfo? _account;
        private readonly IBinanceRestClient _binanceRestClient;
        private readonly IDbContextFactory<BinanceContext> _contextFactory;
        private readonly ILogger<AccountInfoService> _logger;

        public AccountInfoService(IBinanceRestClient binanceRestClient, IDbContextFactory<BinanceContext> contextFactory, ILogger<AccountInfoService> logger) : 
            base("0 */15 * * * *", TimeZoneInfo.Utc, logger)
        {
            _binanceRestClient = binanceRestClient;
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            var result = await _binanceRestClient.SpotApi.Account.GetAccountInfoAsync();
            if (result.Success)
            {
                Account = result.Data;
                _logger.LogInformation($"Updated account info");
            }
        }

        public async Task RefreshIfOlderThan(TimeSpan timeSpan)
        {
            if(Updated.Add(timeSpan) < DateTimeOffset.UtcNow)
            {
                await DoWork(CancellationToken.None);
            }
        }

        public async Task<decimal> GetAvailableBalance(string symbol)
        {
            await RefreshIfOlderThan(TimeSpan.FromMinutes(1));

            if (Account?.Balances != null)
            {
                return Account.Balances.FirstOrDefault(x => x.Asset == symbol.AsBaseAsset())?.Available ?? 0m;
            }

            return 0m;
        }

        public async Task<decimal> GetTotalBalance(string symbol)
        {
            await RefreshIfOlderThan(TimeSpan.FromMinutes(1));

            if (Account?.Balances != null)
            {
                return Account.Balances.FirstOrDefault(x => x.Asset == symbol.AsBaseAsset())?.Total ?? 0m;
            }

            return 0m;
        }

        public async Task<IEnumerable<string>> GetOwnedSymbols()
        {
            await RefreshIfOlderThan(TimeSpan.FromMinutes(15));
            return Account?.Balances?.Where(x => x.Total > 0 && x.Asset != AssetExtensions.QuoteAsset).Select(x => x.Asset).ToArray() ?? new string[0];
        }

        public async Task<decimal> GetTotalValue()
        {
            var ownedSymbols = await GetOwnedSymbols();
            var context = _contextFactory.CreateDbContext();
            var prices = context.Prices.Include(x => x.Crypto).LatestData().ToList();
            var total = 0m;
            foreach(var symbol in ownedSymbols)
            {
                var price = prices.FirstOrDefault(x => x.Crypto.Symbol == symbol.AsSymbolPair());
                if(price != null)
                {
                    var asset = Account?.Balances.FirstOrDefault(x => x.Asset == symbol.AsBaseAsset());
                    if(asset != null)
                    {
                        total += price.Close * asset.Total;
                    }
                }
            }

            return total;
        }
    }
    
}

using Binance.Net.Interfaces.Clients;
using Binance.Net.Objects.Models.Futures;
using Binance.Net.Objects.Models.Spot;
using CryptoTrader.Data.Extensions;

namespace CryptoTrader.Web.Services
{
    public class ExchangeInfoService : CronService
    {
        public BinanceExchangeInfo? ExchangeInfo
        {
            get
            {
                return _exchangeInfo;
            }
            private set
            {
                _exchangeInfo = value;
                Updated = DateTimeOffset.UtcNow;
            }
        }
        public DateTimeOffset? Updated { get; private set; }

        private BinanceExchangeInfo? _exchangeInfo;
        private readonly IBinanceRestClient _binanceRestClient;
        private readonly ILogger<AccountInfoService> _logger;

        public ExchangeInfoService(IBinanceRestClient binanceRestClient, ILogger<AccountInfoService> logger) : 
            base("0 */15 * * * *", TimeZoneInfo.Utc, logger)
        {
            _binanceRestClient = binanceRestClient;
            _logger = logger;
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            var result = await _binanceRestClient.SpotApi.ExchangeData.GetExchangeInfoAsync();
            if (result.Success)
            {
                ExchangeInfo = result.Data;
                _logger.LogInformation($"Updated exchange info");
            }
            
        }

        public async Task RefreshIfOlderThan(TimeSpan timeSpan)
        {
            if(Updated == null || Updated.Value.Add(timeSpan) < DateTimeOffset.UtcNow)
            {
                await DoWork(CancellationToken.None);
            }
        }

        public async Task<BinanceSymbol?> GetSymbolInfo(string symbol)
        {
            await RefreshIfOlderThan(TimeSpan.FromHours(24));
            return ExchangeInfo?.Symbols?.FirstOrDefault(x => x.Name == symbol.AsSymbolPair());
        }
    }
    
}

using CryptoTrader.Data;
using CryptoTrader.Data.Extensions;
using CryptoTrader.Web.Models;
using CryptoTrader.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrader.Web.Controllers
{
    [Route("api/assets")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly AccountInfoService _accountInfoService;
        private readonly BinanceContext _context;
        private readonly PriceHourUpdateService _priceHourUpdateService;
        private readonly FeatureCalculationService _featureCalculationService;
        public AssetsController(AccountInfoService accountInfoService, BinanceContext context, PriceHourUpdateService priceHourUpdateService, FeatureCalculationService featureCalculationService)
        {
            _accountInfoService = accountInfoService;
            _context = context;
            _priceHourUpdateService = priceHourUpdateService;
            _featureCalculationService = featureCalculationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AssetResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var cryptos = await _context.Cryptos.Where(x => x.Active).AsNoTracking().ToListAsync();
            var prices = _context.Prices
                .Include(x => x.Crypto)
                .Include(x => x.MA)
                .Include(x => x.Trend)
                .AsNoTracking()
                .LatestData()
                .ToList();
            await _accountInfoService.RefreshIfOlderThan(TimeSpan.FromMinutes(5));
            var assets = _accountInfoService.Account.Balances.ToList();
            var symbols = cryptos.Select(x => x.Symbol.AsBaseAsset()).Union(assets.Select(x => x.Asset))
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            var ownedSymbols = (await _accountInfoService.GetOwnedSymbols()).Select(x => x.AsSymbolPair()).ToArray();
            var purchases = _context.Orders
                .Where(x => x.Side == OrderSide.Buy && ownedSymbols.Contains(x.Symbol) && x.Status == Data.OrderStatus.Filled)
                .OrderByDescending(x => x.Created)
                .AsNoTracking()
                .ToList();
            var response = new List<AssetResponse>();
            foreach (var symbol in symbols)
            {
                var asset = assets.FirstOrDefault(x => x.Asset == symbol);
                var crypto = cryptos.FirstOrDefault(x => x.Symbol == symbol.AsSymbolPair());
                var price = prices.FirstOrDefault(x => x.Crypto.Symbol == symbol.AsSymbolPair());

                if ((asset == null || asset.Total == 0m) && crypto == null)
                {
                    continue;
                }

                var model = AssetResponse.Create(symbol, crypto, price, asset);

                var latestPurchase = purchases.FirstOrDefault(x => x.Symbol == model.Symbol.AsSymbolPair());
                if (latestPurchase != null)
                {
                    model.LatestPurchasePrice = latestPurchase.AverageFillPrice ?? latestPurchase.Price;
                }
                response.Add(model);
            }

            return Ok(response);
        }

        [HttpPost("{symbol}/follow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Follow(string symbol)
        {
            var crypto = _context.Cryptos.FirstOrDefault(x => x.Symbol == symbol.AsSymbolPair());
            if (crypto == null)
            {
                return NotFound();
            }

            crypto.Followed = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync();

            await _priceHourUpdateService.UpdatePrices(symbol);

            return Ok();
        }

        [HttpPost("{symbol}/unfollow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Unfollow(string symbol)
        {
            var crypto = _context.Cryptos.FirstOrDefault(x => x.Symbol == symbol.AsSymbolPair());
            if (crypto == null)
            {
                return NotFound();
            }

            crypto.Followed = null;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{symbol}/trade/enable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EnableTrade(string symbol)
        {
            var crypto = _context.Cryptos.FirstOrDefault(x => x.Symbol == symbol.AsSymbolPair());
            if (crypto == null)
            {
                return NotFound();
            }

            crypto.Trade = true;
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpPost("{symbol}/trade/disable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DisableTrade(string symbol)
        {
            var crypto = _context.Cryptos.FirstOrDefault(x => x.Symbol == symbol.AsSymbolPair());
            if (crypto == null)
            {
                return NotFound();
            }

            crypto.Trade = false;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{symbol}/prices/update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePrices(string symbol)
        {
            await _priceHourUpdateService.UpdatePrices(symbol);
            return Ok();
        }

        [HttpPost("{symbol}/features/calculate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CalculateFeatures(string symbol)
        {
            await _featureCalculationService.CalculateFeatures(symbol);
            return Ok();
        }
    }
}

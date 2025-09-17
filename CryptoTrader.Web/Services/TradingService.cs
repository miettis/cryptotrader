using Binance.Net.Enums;
using Binance.Net.Interfaces.Clients;
using CryptoExchange.Net.CommonObjects;
using CryptoTrader.Data;
using CryptoTrader.Data.Extensions;
using CryptoTrader.Web.Events;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderSide = Binance.Net.Enums.OrderSide;

namespace CryptoTrader.Web.Services
{
    public class TradingService : CronService
    {
        private static readonly decimal DefaultProfitFactor = 1.03m;
        private static readonly decimal DefaultDropFactor = 0.98m;

        private readonly ILogger<TradingService> _logger;
        private readonly IBinanceRestClient _binanceRestClient;
        private readonly IDbContextFactory<BinanceContext> _contextFactory;
        private readonly AccountInfoService _accountInfoService;
        private readonly OpenOrderService _orderService;
        private readonly ExchangeInfoService _exchangeInfoService;
        private readonly DateTimeOffset _latestTradeCheck = DateTimeOffset.MinValue;
        private bool _running = false;
        public TradingService(ILogger<TradingService> logger, IBinanceRestClient binanceRestClient, IDbContextFactory<BinanceContext> contextFactory, AccountInfoService accountInfoService, OpenOrderService orderService, ExchangeInfoService exchangeInfoService): 
            base("30 5 * * * *", TimeZoneInfo.Utc, logger)
        {
            _logger = logger;
            _binanceRestClient = binanceRestClient;
            _contextFactory = contextFactory;
            _accountInfoService = accountInfoService;
            _orderService = orderService;
            _exchangeInfoService = exchangeInfoService;
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            var ownedSymbols = (await _accountInfoService.GetOwnedSymbols()).Select(x => x.AsSymbolPair()).ToArray();
            var context = await _contextFactory.CreateDbContextAsync();

            var cryptos = context.Cryptos.Where(x => x.Trade || ownedSymbols.Contains(x.Symbol)).ToList();
            await _orderService.RefreshIfOlderThan(TimeSpan.FromMinutes(1));
            
            var totalValue = await _accountInfoService.GetTotalValue();
            foreach(var crypto in cryptos)
            {
                await CheckTradingOpportunities(crypto);
            }
        }

        public async Task CheckTradingOpportunities(Crypto crypto)
        {
            var ownedSymbols = (await _accountInfoService.GetOwnedSymbols()).Select(x => x.AsSymbolPair()).ToArray();

            if (!crypto.Trade && !ownedSymbols.Contains(crypto.Symbol))
            {
                return;
            }

            await _orderService.RefreshIfOlderThan(TimeSpan.FromMinutes(1));

            var currentHourStart = DateTimeOffset.UtcNow.StartOfHour();
            var previousHourStart = currentHourStart.AddHours(-1);
            var context = await _contextFactory.CreateDbContextAsync();
            var usdt = await _accountInfoService.GetAvailableBalance(AssetExtensions.QuoteAsset);
            var cryptoAvailableBalance = await _accountInfoService.GetAvailableBalance(crypto.Symbol.AsBaseAsset());
            var cryptoTotalBalance = await _accountInfoService.GetTotalBalance(crypto.Symbol.AsBaseAsset());
            var totalValue = await _accountInfoService.GetTotalValue();
            var latestPrice = context.Prices.FirstOrDefault(x => x.Crypto.Symbol == crypto.Symbol && x.TimeOpen == previousHourStart);
            var prediction = context.Predictions.FirstOrDefault(x => x.Crypto.Symbol == crypto.Symbol && x.TimeOpen == currentHourStart);
            if (latestPrice == null || prediction == null)
            {
                _logger.LogInformation($"Skipping trading opportunity check | Missing data | latest price: {latestPrice?.TimeOpen}, prediction: {prediction?.TimeOpen}");
                return;
            }

            var openOrders = context.Orders.Where(x => x.Symbol == crypto.Symbol && x.Status == Data.OrderStatus.New).ToList();

            var cryptoAvailableValue = cryptoAvailableBalance * latestPrice.Close;
            var cryptoTotalValue = cryptoTotalBalance * latestPrice.Close;

            if (cryptoAvailableValue >= 5)
            {
                _logger.LogInformation($"Check sell opportunity | {crypto.Symbol} | USDT: {usdt} | {prediction.Day.Rank2}/{prediction.Day.Rank3}/{prediction.Day.Rank4}/{prediction.Day.Rank6}{prediction.Day.Rank8}/{prediction.Day.Rank12} | high: {prediction.High}");

                var buys = context.Orders.Where(x => x.Symbol == crypto.Symbol && x.Side == Data.OrderSide.Buy && x.UnmatchedQuantity > 0).ToList();
                var buyValue = buys.Sum(x => (x.UnmatchedQuantity ?? 0) * (x.AverageFillPrice ?? x.Price));
                var buyQuantity = buys.Sum(x => x.UnmatchedQuantity ?? 0);

                var buyPrice = latestPrice.Avg.HL;

                if (buyQuantity > 0m)
                {
                    buyPrice = buyValue / buyQuantity;
                }
                else
                {
                    var latestBuy = context.Orders.Where(x => x.Symbol == crypto.Symbol && x.Side == Data.OrderSide.Buy && x.Status == Data.OrderStatus.Filled).OrderByDescending(x => x.Created).First();
                    buyPrice = latestBuy.AverageFillPrice ?? latestBuy.Price;
                }


                if (!prediction.GoodTimeToBuy())
                {
                    var price = new[] 
                    { 
                        latestPrice.High,
                        (latestPrice.High + prediction.High) / 2, 
                        buyPrice * DefaultProfitFactor
                    }.Max();


                    _logger.LogInformation($"Sell {crypto.Symbol} {cryptoAvailableBalance} x {price} | latest high: {latestPrice.High} prediction high: {prediction.High} buy: {buyPrice}");

                    await PlaceOrder(crypto.Symbol, price, cryptoAvailableBalance, OrderSide.Sell);
                }
            }
            else if (usdt >= 10 && crypto.Trade && cryptoTotalValue < 5)
            {
                _logger.LogInformation($"Check buy opportunity | {crypto.Symbol} | USDT: {usdt} | {prediction.Day.Rank2}/{prediction.Day.Rank3}/{prediction.Day.Rank4}/{prediction.Day.Rank6}/{prediction.Day.Rank8}/{prediction.Day.Rank12} | low: {prediction.Low} | available: {cryptoAvailableBalance} total: {cryptoTotalBalance} | wallet total: {totalValue}");

                

                if (prediction.GoodTimeToBuy())
                {
                    var latestSell = context.Orders.Where(x => x.Symbol == crypto.Symbol && x.Side == Data.OrderSide.Sell && x.Status == Data.OrderStatus.Filled).OrderByDescending(x => x.Created).First();

                    var price = Math.Min(latestPrice.Low, (latestPrice.Low + prediction.Low) / 2);
                    // pay less than previous sell
                    if(latestSell != null && latestSell.Updated > DateTimeOffset.Now.AddDays(-2))
                    {
                        price = Math.Min(price, DefaultDropFactor * (latestSell.AverageFillPrice ?? latestSell.Price));
                    }
                    var usdtToSpend = usdt;
                    if (crypto.MaxTotal.HasValue)
                    {
                        usdtToSpend = Math.Min(usdt, crypto.MaxTotal.Value - cryptoTotalValue);
                    }
                    if (crypto.MaxPurchase.HasValue)
                    {
                        usdtToSpend = Math.Min(usdtToSpend, crypto.MaxPurchase.Value);
                    }
                    var quantity = usdtToSpend / price;

                    _logger.LogInformation($"Buy {crypto.Symbol} {quantity} x {price} | latest low: {latestPrice.Low} prediction low: {prediction.Low}");

                    await PlaceOrder(crypto.Symbol, price, quantity, OrderSide.Buy);
                }
            }
        }

        public async Task<Data.Order?> BuyLow(string symbol, decimal maxUSDT)
        {
            await CancelOpenOrders(symbol);
            
            var usdt = await _accountInfoService.GetAvailableBalance(AssetExtensions.QuoteAsset);
            usdt = Math.Min(usdt, maxUSDT);
            var context = await _contextFactory.CreateDbContextAsync();
            var latestTime = DateTimeOffset.UtcNow.StartOfHour().AddHours(-1);
            var latestPrice = context.Prices.Where(x => x.Crypto.Symbol == symbol.AsSymbolPair() && x.TimeOpen == latestTime).OrderByDescending(x => x.TimeOpen).FirstOrDefault();
            if (latestPrice == null || usdt < 10)
            {
                return null;
            }
       
            var price = latestPrice.Low;
            var quantity = usdt / price;

            return await PlaceOrder(symbol.AsSymbolPair(), price, quantity, OrderSide.Buy);
        }

        public async Task<Data.Order?> BuyMA24Std(string symbol, decimal maxUSDT)
        {
            await CancelOpenOrders(symbol);

            var usdt = await _accountInfoService.GetAvailableBalance(AssetExtensions.QuoteAsset);
            usdt = Math.Min(usdt, maxUSDT);
            var context = await _contextFactory.CreateDbContextAsync();
            var latestTime = DateTimeOffset.UtcNow.StartOfHour().AddHours(-1);
            var latestPrice = context.Prices.Include(x => x.MA).Where(x => x.Crypto.Symbol == symbol.AsSymbolPair() && x.TimeOpen == latestTime).OrderByDescending(x => x.TimeOpen).FirstOrDefault();
            if (latestPrice == null || usdt < 10 || latestPrice.MA?.SMA24.Sma == null || latestPrice.MA?.SMA24.Mad == null)
            {
                return null;
            }

            var price = latestPrice.MA.SMA24.Sma.Value - latestPrice.MA.SMA24.Mad.Value;
            if(price > latestPrice.Avg.HL)
            {
                // if calculated values are wrong/missing don't buy above avg
                return null;
            }
            var quantity = usdt / price;

            return await PlaceOrder(symbol.AsSymbolPair(), price, quantity, OrderSide.Buy);
        }

        public async Task<Data.Order?> SellHigh(string symbol)
        {
            await CancelOpenOrders(symbol);

            var cryptoBalance = await _accountInfoService.GetAvailableBalance(symbol);
            var context = await _contextFactory.CreateDbContextAsync();
            var latestTime = DateTimeOffset.UtcNow.StartOfHour().AddHours(-1);
            var latestPrice = context.Prices.Where(x => x.Crypto.Symbol == symbol.AsSymbolPair() && x.TimeOpen == latestTime).OrderByDescending(x => x.TimeOpen).FirstOrDefault();
            if (latestPrice == null || cryptoBalance == 0)
            {
                return null;
            }
            var latestBuy = context.Orders.Where(x => x.Symbol == symbol.AsSymbolPair() && x.Side == Data.OrderSide.Buy).OrderByDescending(x => x.Created).FirstOrDefault();
            if(latestBuy == null)
            {
                return null;
            }
            var price = Math.Max(DefaultProfitFactor * latestBuy.Price, latestPrice.High);
            
            return await PlaceOrder(symbol.AsSymbolPair(), price, cryptoBalance, OrderSide.Sell);
        }

        public async Task<Data.Order?> SellMA24Std(string symbol)
        {
            await CancelOpenOrders(symbol);

            var cryptoBalance = await _accountInfoService.GetAvailableBalance(symbol);
            var context = await _contextFactory.CreateDbContextAsync();
            var latestTime = DateTimeOffset.UtcNow.StartOfHour().AddHours(-1);
            var latestPrice = context.Prices.Include(x => x.MA).Where(x => x.Crypto.Symbol == symbol.AsSymbolPair() && x.TimeOpen == latestTime).OrderByDescending(x => x.TimeOpen).FirstOrDefault();
            if (latestPrice == null || cryptoBalance == 0 || latestPrice.MA?.SMA24.Sma == null || latestPrice.MA?.SMA24.Mad == null)
            {
                return null;
            }
            var latestBuy = context.Orders.Where(x => x.Symbol == symbol.AsSymbolPair() && x.Side == Data.OrderSide.Buy).OrderByDescending(x => x.Created).FirstOrDefault();
            if (latestBuy == null)
            {
                return null;
            }
            var price = latestPrice.MA.SMA24.Sma.Value + latestPrice.MA.SMA24.Mad.Value;
            if (price < latestPrice.Avg.HL)
            {
                // if calculated values are wrong/missing don't sell below avg
                return null;
            }
            return await PlaceOrder(symbol.AsSymbolPair(), price, cryptoBalance, OrderSide.Sell);
        }

        public async Task<Data.Order?> SellDefault(Data.Order order)
        {
            if(order.ExecutedQuantity == 0)
            {
                return null;
            }
            /*
            var context = await _contextFactory.CreateDbContextAsync();
            var latestTime = DateTimeOffset.UtcNow.StartOfHour().AddHours(-1);
            var latestPrice = context.Prices.Where(x => x.Crypto.Symbol == order.Symbol.AsSymbolPair()).Include(x => x.MA).OrderByDescending(x => x.TimeOpen).FirstOrDefault();
            if(latestPrice == null || latestPrice.TimeOpen < latestTime.AddHours(-1))
            {
                return null;
            }
            var sellPrice = new[] 
            { 
                DefaultProfitFactor * (order.AverageFillPrice ?? order.Price),
                latestPrice.High 
            }.Max();

            if (latestPrice.MA?.SMA24?.Sma != null && latestPrice.MA?.SMA24?.Mad != null)
            {
                sellPrice = Math.Max(sellPrice, latestPrice.MA.SMA24.Sma.Value + latestPrice.MA.SMA24.Mad.Value);
            }

            await _orderService.RefreshIfOlderThan(TimeSpan.Zero);
            await _accountInfoService.RefreshIfOlderThan(TimeSpan.Zero);

            var sellQuantity = await _accountInfoService.GetAvailableBalance(order.Symbol);

            _logger.LogInformation($"Making default sell order {order.Symbol} {sellQuantity} * {sellPrice} | avg fill: {order.AverageFillPrice}, latest high: {latestPrice.High}, SMA: {latestPrice.MA?.SMA24?.Sma} MAD: {latestPrice.MA?.SMA24?.Mad}");
            return await PlaceOrder(order.Symbol, sellPrice, sellQuantity, OrderSide.Sell);
            */

            return await SellDefaultProfit(order.Symbol);
        }

        public async Task<Data.Order?> SellDefaultProfit(string symbol)
        {
            var context = await _contextFactory.CreateDbContextAsync();
            var unmatchedBuys = context.Orders.Where(x => x.Symbol == symbol.AsSymbolPair() && x.Side == Data.OrderSide.Buy && x.UnmatchedQuantity > 0).ToList();
            if(unmatchedBuys.Count == 0)
            {
                return null;
            }
            var avgBuyPrice = unmatchedBuys.Sum(x => x.UnmatchedQuantity.Value * (x.AverageFillPrice ?? x.Price)) / unmatchedBuys.Sum(x => x.UnmatchedQuantity.Value);

            var latestTime = DateTimeOffset.UtcNow.StartOfHour().AddHours(-1);
            var latestPrice = context.Prices.Where(x => x.Crypto.Symbol == symbol.AsSymbolPair()).Include(x => x.MA).OrderByDescending(x => x.TimeOpen).FirstOrDefault();
            if (latestPrice == null || latestPrice.TimeOpen < latestTime.AddHours(-1))
            {
                return null;
            }
            var latestBuy = unmatchedBuys.OrderByDescending(x => x.Updated).First();
            var sellPrice = new[]
            {
                DefaultProfitFactor * avgBuyPrice,
                latestPrice.High,
                DefaultProfitFactor * (latestBuy.AverageFillPrice ?? latestBuy.Price)
            }.Max();

            if (latestPrice.MA?.SMA24?.Sma != null && latestPrice.MA?.SMA24?.Mad != null)
            {
                sellPrice = Math.Max(sellPrice, latestPrice.MA.SMA24.Sma.Value + latestPrice.MA.SMA24.Mad.Value);
            }

            await CancelOpenOrders(symbol);

            await _orderService.RefreshIfOlderThan(TimeSpan.Zero);
            await _accountInfoService.RefreshIfOlderThan(TimeSpan.Zero);

            var sellQuantity = await _accountInfoService.GetAvailableBalance(symbol);

            _logger.LogInformation($"Making default profit sell order {symbol} {sellQuantity} * {sellPrice} | avg buy: {avgBuyPrice}, latest high: {latestPrice.High}, SMA: {latestPrice.MA?.SMA24?.Sma} MAD: {latestPrice.MA?.SMA24?.Mad}");
            return await PlaceOrder(symbol.AsSymbolPair(), sellPrice, sellQuantity, OrderSide.Sell);
        }


        public async Task<Data.Order?> CancelOrder(int id)
        {
            var context = _contextFactory.CreateDbContext();
            var order = context.Orders.First(x => x.Id == id);
            if(order.Status != Data.OrderStatus.Filled)
            {
                var response = await _binanceRestClient.SpotApi.Trading.CancelOrderAsync(order.Symbol, order.BinanceId);
                if (response.Success)
                {
                    order.Status = (Data.OrderStatus)response.Data.Status;
                    order.CancelResponse = JsonConvert.SerializeObject(response.Data);

                    await context.SaveChangesAsync();
                }
            }

            await _orderService.RefreshIfOlderThan(TimeSpan.Zero);
            await _accountInfoService.RefreshIfOlderThan(TimeSpan.Zero);

            return order;
        }


        private async Task<Data.Order?> PlaceOrder(string symbol, decimal price, decimal quantity, OrderSide side)
        {
            await CancelOpenOrders(symbol);

            var symbolInfo = await _exchangeInfoService.GetSymbolInfo(symbol);
            
            var finalQuantity = quantity;
            var finalPrice = price;
            var lotFilter = symbolInfo.LotSizeFilter;
            var priceFilter = symbolInfo.PriceFilter;
            if (side == OrderSide.Buy)
            {

                finalQuantity = GetNumber(quantity, lotFilter.MinQuantity, lotFilter.MaxQuantity, lotFilter.StepSize);
                finalPrice = GetNumber(price, priceFilter.MinPrice, priceFilter.MaxPrice, priceFilter.TickSize);
            }
            else
            {
                finalQuantity = GetNumber(quantity, lotFilter.MinQuantity, lotFilter.MaxQuantity, lotFilter.StepSize);
                finalPrice = GetNumber(price, priceFilter.MinPrice, priceFilter.MaxPrice, priceFilter.TickSize);
            }
            var result = await _binanceRestClient.SpotApi.Trading.PlaceOrderAsync(
            symbol: symbol.AsSymbolPair(),
            side: side,
                type: SpotOrderType.Limit, 
                quantity: finalQuantity, 
                price: finalPrice,
                timeInForce: TimeInForce.GoodTillCanceled);

            if (result.Success)
            {
                var order = new Data.Order
                {
                    Created = result.Data.CreateTime,
                    Updated = result.Data.UpdateTime ?? result.Data.CreateTime,
                    BinanceId = result.Data.Id,
                    Status = (Data.OrderStatus)result.Data.Status,
                    Side = (Data.OrderSide)side,
                    Symbol = symbol,
                    Price = finalPrice,
                    Quantity = finalQuantity,
                    ExecutedQuantity = result.Data.QuantityFilled,
                    QuoteQuantity = result.Data.QuoteQuantityFilled,
                    Commission = result.Data.Trades != null ? result.Data.Trades.Sum(x => x.Fee) : null,
                    AverageFillPrice = result.Data.AverageFillPrice,
                    RemainingQuantity = result.Data.QuantityRemaining,
                    CreateResponse = JsonConvert.SerializeObject(result.Data),
                    UnmatchedQuantity = side == OrderSide.Buy ? 0.999m * finalQuantity : finalQuantity
                };

                var context = await _contextFactory.CreateDbContextAsync();
                await context.Orders.AddAsync(order);
                await context.SaveChangesAsync();

                await new OrderCreatedEvent
                {
                    Symbol = symbol,
                    Side = side,
                    BinanceId = order.BinanceId
                }.PublishAsync(Mode.WaitForNone);


                if(side == OrderSide.Buy && result.Data.Status == Binance.Net.Enums.OrderStatus.Filled)
                {
                    await SellDefault(order);
                }

                return order;
            }

            return null;
        }
        
        private async Task CancelOpenOrders(string symbol)
        {
            var result = await _binanceRestClient.SpotApi.Trading.CancelAllOrdersAsync(symbol.AsSymbolPair());
            if (result.Success)
            {
                var ids = result.Data.Select(x => x.Id).ToList();
                var context = await _contextFactory.CreateDbContextAsync();
                var orders = await context.Orders.Where(x => ids.Contains(x.BinanceId)).ToListAsync();
                foreach (var data in result.Data)
                {
                    var order = orders.FirstOrDefault(x => x.BinanceId == data.Id);
                    if (order != null)
                    {
                        order.Status = (Data.OrderStatus)data.Status;
                        order.CancelResponse = JsonConvert.SerializeObject(data);
                    }
                }

                await context.SaveChangesAsync();

                await _orderService.RefreshIfOlderThan(TimeSpan.Zero);
                await _accountInfoService.RefreshIfOlderThan(TimeSpan.Zero);
            }
        }


        private decimal GetNumber(decimal value, decimal min, decimal max, decimal step)
        {
            if(value < min)
            {
                return min;
            }
            if(value > max)
            {
                return max;
            }

            var multiplier = Math.Truncate(value / step);
            return multiplier * step;
        }
    }
}

using CryptoTrader.Data;
using CryptoTrader.Web.Models;
using CryptoTrader.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrader.Web.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly BinanceContext _context;
        private readonly TradingService _tradingService;
        private readonly OrderUpdateService _orderUpdateService;
        public OrdersController(BinanceContext context, TradingService tradingService, OrderUpdateService orderUpdateService)
        {
            _context = context;
            _tradingService = tradingService;
            _orderUpdateService = orderUpdateService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Query([FromQuery] string? symbol, [FromQuery] DateTimeOffset start, [FromQuery] DateTimeOffset end)
        {
            await _orderUpdateService.UpdateOpenOrders();

            var query = _context.Orders.AsQueryable();
            if (!string.IsNullOrEmpty(symbol))
            {
                query = query.Where(x => x.Symbol == symbol);
            }
            var orders = query.Where(x => x.Status != OrderStatus.Canceled)
                .Include(x => x.SellOrders)
                .OrderByDescending(x => x.Created)
                .ToList();
            var response = orders.Select(OrderResponse.Create).ToList();
            foreach(var sell in orders.Where(x => x.Side == OrderSide.Sell))
            {
                var buyValue = 0m;
                var buyQty = 0m;
                foreach(var buy in orders.Where(x => x.Side == OrderSide.Buy))
                {
                    foreach(var sellOrder in buy.SellOrders.Where(x => x.SellOrderId == sell.Id))
                    {
                        buyValue += sellOrder.Quantity * (buy.AverageFillPrice ?? buy.Price);
                        buyQty += sellOrder.Quantity;
                    }
                }
                if(buyQty == 0)
                {
                    continue;
                }
                var buyAvg = buyValue / buyQty;
                var sellPrice = sell.AverageFillPrice.Value;
                
                var dto = response.First(x => x.Id == sell.Id);
                dto.Profit = Math.Round((sellPrice - buyAvg) / buyAvg * 100, 2);
            }
            response = response.ToList();
            return Ok(response);
        }

        [HttpPost("cancel/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Cancel(int id)
        {
            await _tradingService.CancelOrder(id);
            return Ok();
        }

        [HttpPost("buy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Buy([FromBody] BuyRequest request)
        {
            if (request.Type == BuyType.Low)
            {
                await _tradingService.BuyLow(request.Symbol, request.USDT);
            }
            else if (request.Type == BuyType.MA24Std)
            {
                await _tradingService.BuyMA24Std(request.Symbol, request.USDT);
            }

            return Ok();
        }


        [HttpPost("sell")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Sell([FromBody] SellRequest request)
        {
            if (request.Type == SellType.High)
            {
                await _tradingService.SellHigh(request.Symbol);
            }
            else if (request.Type == SellType.MA24Std)
            {
                await _tradingService.SellMA24Std(request.Symbol);
            }
            else if(request.Type == SellType.DefaultProfit)
            {
                await _tradingService.SellDefaultProfit(request.Symbol);
            }

            return Ok();
        }
    }
}

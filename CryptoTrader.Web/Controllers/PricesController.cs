using CryptoTrader.Data;
using CryptoTrader.Data.Extensions;
using CryptoTrader.Data.Features;
using CryptoTrader.Data.Services;
using CryptoTrader.Web.Models;
using CryptoTrader.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CryptoTrader.Web.Controllers
{
    [Route("api/prices")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private readonly BinanceContext _context;
        private readonly PriceHourStreamService _priceHourStream;
        private readonly PythonService _predictionService;
        public PricesController(BinanceContext context, PriceHourStreamService priceHourStream, PythonService predictionService)
        {
            _context = context;
            _priceHourStream = priceHourStream;
            _predictionService = predictionService;
        }

        [HttpGet("hour/latest")]
        [ProducesResponseType(typeof(IEnumerable<PriceHourResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetHourLatest()
        {
            var prices = _context.Prices
                .Include(x => x.Crypto)
                .Include(x => x.MA)
                .Include(x => x.Trend)
                .LatestData()
                .OrderBy(x => x.Crypto.Symbol)
                .ToList();
            var response = prices.Select(PriceHourResponse.Create).ToList();
            return Ok(response);
        }

        [HttpGet("hour")]
        [ProducesResponseType(typeof(IEnumerable<PriceHourResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> QueryHour([FromQuery] string symbol, [FromQuery] DateTimeOffset start, [FromQuery] DateTimeOffset end)
        {
            var prices = _context.Prices
                .Include(x => x.Crypto)
                .Include(x => x.MA)
                .Include(x => x.Trend)
                .Include(x => x.Return)
                .Include(x => x.Cycles)
                .Include(x => x.Momentum)
                .Include(x => x.Peak)
                .Include(x => x.OtherIndicators)
                .Include(x => x.Volatility)
                .Include(x => x.Volumes)
                .Where(x => x.Crypto.Symbol == symbol && x.TimeOpen >= start && x.TimeOpen < end)
                .OrderBy(x => x.TimeOpen)
                .ToList();

            var response = prices.Select(PriceHourResponse.Create).ToList();

            var partialPrice = _priceHourStream.GetLatestPrice(symbol);
            if (partialPrice != null && partialPrice.TimeOpen >= start && partialPrice.TimeOpen < end && (prices.Count == 0 || partialPrice.TimeOpen > prices.Last().TimeOpen))
            {
                var partialResponse = PriceHourResponse.Create(partialPrice);

                response.Add(partialResponse);
            }

            var predictions = _context.Predictions.Where(x => x.Crypto.Symbol == symbol && x.TimeOpen >= start && x.TimeOpen < end).ToList();
            foreach (var prediction in predictions)
            {
                var resp = response.FirstOrDefault(x => x.TimeOpen == prediction.TimeOpen);
                if (resp != null)
                {
                    resp.PredictionHigh = prediction.High;
                    resp.PredictionLow = prediction.Low;
                }
            }

            return Ok(response);
        }
        /*
        [HttpGet("features")]
        [ProducesResponseType(typeof(IEnumerable<PriceHourResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> QueryFeatures([FromQuery] string symbol, [FromQuery] DateTimeOffset start, [FromQuery] DateTimeOffset end, [FromQuery] FeatureCategory category)
        {
            Func<Price, object> includeProperty = category switch
            {
                FeatureCategory.Cycle => x => x.Cycles,
                FeatureCategory.Momentum => x => x.Momentum,
                FeatureCategory.MovingAverage => x => x.MA,
                FeatureCategory.Other => x => x.OtherIndicators,
                FeatureCategory.Peak => x => x.Peak,
                FeatureCategory.Return => x => x.Return,
                FeatureCategory.Trend => x => x.Trend,
                FeatureCategory.Volatility => x => x.Volatility,
                FeatureCategory.Volume => x => x.Volume,
                _ => throw new NotImplementedException()
            };  

            var prices = _context.Prices
                .Include(includeProperty)
                .Where(x => x.Crypto.Symbol == symbol && x.TimeOpen >= start && x.TimeOpen < end)
                .OrderBy(x => x.TimeOpen)
                .ToList();

            var response = prices.Select(includeProperty).ToList();


            return Ok(response);
        }
        */
        /*
        [HttpGet("feature/{featureName}")]
        [ProducesResponseType(typeof(IEnumerable<PriceHourResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> QueryFeature(string featureName, [FromQuery] string symbol, [FromQuery] DateTimeOffset start, [FromQuery] DateTimeOffset end)
        {
            var group = FeatureConfig.FeatureGroups.First(x => x.Features.Any(f => f.Name == featureName));
            if(group == null)
            {
                return NotFound();
            }
            var prices = _context.Prices
                .Include(x => x.Crypto)
                .Include(group.IncludeProperty)
                .Where(x => x.Crypto.Symbol == symbol && x.TimeOpen >= start && x.TimeOpen < end)
                .OrderBy(x => x.TimeOpen)
                .ToList();

            var result = new List<FeatureResponse>();
            foreach(var price in prices)
            {
                var groupValue = group.Property.GetValue(price);
                var feature = group.Features.First(x => x.Name == featureName);

                result.Add(new FeatureResponse
                {
                    TimeOpen = price.TimeOpen,
                    Value = feature.Property.GetValue(groupValue)
                });
            }

            return Ok(result);
        }
        */
        /*
        [HttpGet("minute/latest")]
        [ProducesResponseType(typeof(IEnumerable<PriceMinuteResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMinuteLatest()
        {
            var prices = _context.PriceMinute.Include(x => x.Crypto)
                .LatestData()
                .OrderBy(x => x.Crypto.Symbol)
                .ToList();
            var response = prices.Select(PriceMinuteResponse.Create).ToList();
            return Ok(response);
        }

        [HttpGet("minute")]
        [ProducesResponseType(typeof(IEnumerable<PriceMinuteResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> QueryMinute([FromQuery] string symbol, [FromQuery] DateTimeOffset start, [FromQuery] DateTimeOffset end)
        {
            var prices = _context.PriceMinute
                .Where(x => x.Crypto.Symbol == symbol && x.TimeOpen >= start && x.TimeOpen < end)
                .OrderBy(x => x.TimeOpen)
                .ToList();
            var response = prices.Select(PriceMinuteResponse.Create).ToList();
            return Ok(response);
        } 
        */
    }
}

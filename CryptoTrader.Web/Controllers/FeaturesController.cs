using CryptoTrader.Data;
using CryptoTrader.Data.Features;
using CryptoTrader.Data.Services;
using CryptoTrader.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoTrader.Web.Controllers
{
    [Route("api/features")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
        public FeaturesController()
        {
        }

        /*
        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<FeatureGroup>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFeatures()
        {
            var features = FeatureConfig.FeatureGroups;
            return Ok(features);
        }
        */
    }
}

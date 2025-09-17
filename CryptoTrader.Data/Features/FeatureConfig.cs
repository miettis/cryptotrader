using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Data.Features
{
    public class FeatureConfig
    {
        public static List<FeatureGroup> FeatureGroups { get; private set; }
        static FeatureConfig()
        {
            FeatureGroups = new List<FeatureGroup>
            {
                new()
                {
                    Name = "Cycle",
                    Property = typeof(Price).GetProperty(nameof(Price.Cycles)),
                    IncludeProperty = x => x.Cycles,
                    Features = GetFeatures<PriceCycles>().ToList()
                },
                new()
                {
                    Name = "Momentum",
                    Property = typeof(Price).GetProperty(nameof(Price.Momentum)),
                    IncludeProperty = x => x.Momentum,
                    Features = GetFeatures<PriceMomentums>().ToList()
                },
                new()
                {
                    Name = "Moving average",
                    Property = typeof(Price).GetProperty(nameof(Price.MA)),
                    IncludeProperty = x => x.MA,
                    Features = GetFeatures<PriceMovingAverages>().ToList()
                },
                new()
                {
                    Name = "Trend",
                    Property = typeof(Price).GetProperty(nameof(Price.Trend)),
                    IncludeProperty = x => x.Trend,
                    Features = GetFeatures<PriceTrends>().ToList()
                },
                new()
                {
                    Name = "Volatility",
                    Property = typeof(Price).GetProperty(nameof(Price.Volatility)),
                    IncludeProperty = x => x.Volatility,
                    Features = GetFeatures<PriceVolatilities>().ToList()
                },
                new()
                {
                    Name = "Volume",
                    Property = typeof(Price).GetProperty(nameof(Price.Volume)),
                    IncludeProperty = x => x.Volumes,
                    Features = GetFeatures<PriceVolumes>().ToList()
                }
            };
        }

        private static IEnumerable<Feature> GetFeatures<T>() where T : FeatureContainer
        {
            foreach(var property in typeof(T).GetProperties())
            {
                if(property.Name == "Id" || property.Name == "Price")
                {
                    continue;
                }

                yield return new Feature
                {
                    Name = property.Name,
                    Property = property
                };
            }
        }
    }
    public class FeatureGroup
    {
        public string Name { get; set; }
        public PropertyInfo Property { get; set; }
        public Expression<Func<Price,FeatureContainer>> IncludeProperty { get; set; }
        public List<Feature> Features { get; set; }
    }
    public class Feature
    {
        public string Name { get; set; }
        public PropertyInfo Property { get; set; }
    }
}

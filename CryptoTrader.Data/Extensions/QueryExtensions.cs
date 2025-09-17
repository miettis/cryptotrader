using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Data.Extensions
{
    public static class QueryExtensions
    {
        public static IQueryable<Price> LatestData(this IQueryable<Price> query)
        {
            var time = DateTimeOffset.UtcNow.StartOfHour().AddHours(-1);
            return query.Where(x => x.TimeOpen == time);
        }
        /*
        public static IQueryable<PriceMinute> LatestData(this IQueryable<PriceMinute> query)
        {
            var time = DateTimeOffset.UtcNow.StartOfMinute().AddMinutes(-1);
            return query.Where(x => x.TimeOpen == time);
        }
        */
    }
}

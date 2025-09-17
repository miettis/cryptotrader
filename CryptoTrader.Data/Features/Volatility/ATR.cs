using Microsoft.EntityFrameworkCore;

namespace CryptoTrader.Data.Features.Volatility
{
    /// <summary>
    /// Average true range
    /// </summary>
    [Owned]
    public class ATR
    {
        public decimal? H12 { get; set; }
        public decimal? H24 { get; set; }
        public decimal? H48 { get; set; }
        public decimal? H168 { get; set; }
    }
}

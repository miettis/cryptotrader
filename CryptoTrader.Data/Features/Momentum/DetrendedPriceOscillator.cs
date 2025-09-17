using Microsoft.EntityFrameworkCore;

namespace CryptoTrader.Data.Features.Momentum
{
    [Owned]
    public class DetrendedPriceOscillator
    {
        public static readonly int DefaultLookbackPeriods = 12;

        [Comment("ignore_prediction")]
        public decimal? Sma { get; set; }
        [Comment("ignore_prediction")]
        public decimal? Dpo { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Momentum
{
    [Owned]
    public class TripleEmaOscillator
    {
        public static readonly int DefaultLookbackPeriods = 12;
        public static readonly int DefaultSignalPeriods = 3;

        [Column("ema3")]
        public decimal? EMA3 { get; set; }

        [Column("trix")]
        public decimal? Trix { get; set; }

        [Column("signal")]
        public decimal? Signal { get; set; }
    }
}

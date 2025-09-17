using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Volatility
{
    [Owned]
    public class StarcBand
    {
        public static readonly int DefaultSmaPeriods = 6;
        public static readonly double DefaultMultiplier = 2.0;
        public static readonly int DefaultAtrPeriods = 8;

        [Column("upper")]
        public decimal? Upper { get; set; }

        [Column("center")]
        public decimal? Center { get; set; }

        [Column("lower")]
        public decimal? Lower { get; set; }
    }
}

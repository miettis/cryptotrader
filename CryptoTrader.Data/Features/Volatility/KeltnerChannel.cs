using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Volatility
{
    [Owned]
    public class KeltnerChannel
    {
        public static readonly int DefaultEmaPeriods = 24;
        public static readonly int DefaultMultiplier = 2;
        public static readonly int DefaultAtrPeriods = 12;

        [Column("upper")]
        public decimal? Upper { get; set; }

        [Column("center")]
        public decimal? Center { get; set; }

        [Column("lower")]
        public decimal? Lower { get; set; }

        [Column("width")]
        public decimal? Width { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Volatility
{
    [Owned]
    public class DonchianChannel
    {
        public static readonly int DefaultLoopbackPeriods = 24;

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

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Momentum
{
    [Owned]
    public class RateOfChange
    {
        public static readonly int DefaultLookbackPeriods = 6;
        public static readonly int DefaultSmaPeriods = 6;

        [Column("momentum")]
        public decimal? Momentum { get; set; }

        [Column("roc")]
        public decimal? Roc { get; set; }

        [Column("sma")]
        public decimal? Sma { get; set; }
    }
}

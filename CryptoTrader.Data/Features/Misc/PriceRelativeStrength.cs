using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Misc
{
    [Owned]
    public class PriceRelativeStrength
    {
        public static readonly int DefaultLoopbackPeriods = 12;
        public static readonly int DefaultSmaPeriods = 12;


        [Column("prs")]
        [Comment("ignore_prediction")]
        public decimal? Prs { get; set; }

        [Column("sma")]
        [Comment("ignore_prediction")]
        public decimal? Sma { get; set; }

        [Column("percent")]
        [Comment("ignore_prediction")]
        public decimal? Percent { get; set; }
    }
}

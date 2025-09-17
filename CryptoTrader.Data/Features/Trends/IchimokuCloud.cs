using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Trends
{
    [Owned]
    public class IchimokuCloud
    {
        public static readonly int DefaultTenkanPeriods = 8;
        public static readonly int DefaultKijunPeriods = 24;
        public static readonly int DefaultSenkouSpanPeriods = 48;



        /// <summary>
        /// TenkanSen | Conversion / signal line
        /// </summary>
        [Column("signal")]
        public decimal? Signal { get; set; }

        /// <summary>
        /// Kijunsen | Base line
        /// </summary>
        [Column("base")]
        public decimal? Base { get; set; }

        /// <summary>
        /// SenkouSpanA | Leading span A
        /// </summary>
        [Column("leada")]
        public decimal? LeadingA { get; set; }

        /// <summary>
        /// SenkouSpanB | Leading span B
        /// </summary>
        [Column("leadb")]
        public decimal? LeadingB { get; set; }

        /// <summary>
        /// ChikouSpan | Lagging span
        /// </summary>
        [IgnoreDataCheck]
        [Column("lag")]
        [Comment("ignore_prediction")]
        public decimal? Lagging { get; set; }
    }
}

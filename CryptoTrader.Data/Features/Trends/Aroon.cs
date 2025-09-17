using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Trends
{
    [Owned]
    public class Aroon
    {
        public static readonly int DefaultLookbackPeriods = 24;

        /// <summary>
        /// Based on last High price
        /// </summary>
        [Column("up")]
        public decimal? Up { get; set; }

        /// <summary>
        /// Based on last Low price
        /// </summary>
        [Column("down")]
        public decimal? Down { get; set; }

        /// <summary>
        /// Up - Down
        /// </summary>
        [Column("osc")]
        public decimal? Oscillator{ get; set; }
    }
}

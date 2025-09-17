using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Momentum
{
    [Owned]
    public class ElderRayIndex
    {
        public static readonly int DefaultLookbackPeriods = 12;

        /// <summary>
        /// Bull Power
        /// </summary>
        [Column("bull")]
        public decimal? Bull { get; set; }

        /// <summary>
        /// Bear Power
        /// </summary>
        [Column("bear")]
        public decimal? Bear { get; set; }
    }
}

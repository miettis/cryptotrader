using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Trends
{
    [Owned]
    public class AverageDirectionalIndex
    {
        public static readonly int DefaultLookbackPeriods = 12;

        /// <summary>
        /// Plus Directional Index
        /// </summary>
        [Column("pdi")]
        public decimal? Pdi { get; set; }

        /// <summary>
        /// Minus Directional Index
        /// </summary>
        [Column("mdi")]
        public decimal? Mdi { get; set; }

        /// <summary>
        /// Average Directional Index
        /// </summary>
        [Column("adx")]
        public decimal? Adx { get; set; }

        /// <summary>
        /// Average Directional Index Rating
        /// </summary>
        [Column("adxr")]
        public decimal? Adxr { get; set; }
    }
}

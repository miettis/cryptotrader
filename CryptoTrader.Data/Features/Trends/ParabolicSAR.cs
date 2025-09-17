using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Trends
{
    [Owned]
    public class ParabolicSAR
    {
        public static readonly double DefaultAccelerationFactor = 0.02;
        public static readonly double DefaultMaxAccelerationFactor = 0.20;

        /// <summary>
        /// Stop and Reverse value
        /// </summary>
        [Column("sar")]
        public decimal? SAR { get; set; }

        /// <summary>
        /// Indicates a trend reversal
        /// </summary>
        [Column("rev")]
        public bool IsReversal { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Momentum
{
    [Owned]
    public class GatorOscillator
    {
        /// <summary>
        /// Absolute value of Alligator Jaw-Teeth
        /// </summary>
        [Column("upper")]
        public decimal? Upper { get; set; }

        /// <summary>
        /// Absolute value of Alligator Lips-Teeth
        /// </summary>
        [Column("lower")]
        public decimal? Lower { get; set; }

        /// <summary>
        /// Upper value is growing
        /// </summary>
        [Column("upper_exp")]
        public bool UpperExpanding { get; set; }

        /// <summary>
        /// Lower value is growing
        /// </summary>
        [Column("lower_exp")]
        public bool LowerExpanding { get; set; }
    }
}

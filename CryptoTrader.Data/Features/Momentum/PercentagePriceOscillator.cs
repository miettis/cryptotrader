using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Momentum
{
    [Owned]
    public class PercentagePriceOscillator
    {
        [Column("ppo")]
        public decimal? Ppo { get; set; }

        [Column("signal")]
        public decimal? Signal { get; set; }

        [Column("hist")]
        public decimal? Histogram { get; set; }
    }
}

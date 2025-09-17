using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Momentum
{
    [Owned]
    public class StochasticFastOscillator
    {
        [Column("ppo")]
        public decimal? Sfo { get; set; }

        [Column("signal")]
        public decimal? Signal { get; set; }
    }
}

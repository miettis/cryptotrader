using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Misc
{
    [Owned]
    public class VerticalHorizontalFilter
    {
        [Column("ppo")]
        public decimal? Vhf { get; set; }

        [Column("signal")]
        public decimal? Signal { get; set; }
    }
}

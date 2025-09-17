using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Misc
{
    [Owned]
    public class ProjectionOscillator
    {
        [Column("pbo")]
        public decimal? Pbo { get; set; }

        [Column("signal")]
        public decimal? Signal { get; set; }
    }
}

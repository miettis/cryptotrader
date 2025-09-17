using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Misc
{
    [Owned]
    public class FractalChaosBand
    {
        public static readonly int DefaultWindowSpan = 2;

        [Column("upper")]
        public decimal? Upper { get; set; }

        [Column("lower")]
        public decimal? Lower { get; set; }
    }
}

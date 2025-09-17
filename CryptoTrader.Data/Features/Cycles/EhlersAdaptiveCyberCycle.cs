using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Cycles
{
    [Owned]
    public class EhlersAdaptiveCyberCycle
    {
        [Column("cycle")]
        public decimal? Cycle { get; set; }

        [Column("period")]
        public decimal? Period { get; set; }
    }
}

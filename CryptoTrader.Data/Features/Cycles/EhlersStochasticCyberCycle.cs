using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Cycles
{
    [Owned]
    public class EhlersStochasticCyberCycle
    {
        [Column("cycle")]
        public decimal? Cycle { get; set; }

        [Column("signal")]
        public decimal? Signal { get; set; }
    }
}

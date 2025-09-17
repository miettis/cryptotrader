using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Cycles
{
    [Owned]
    public class EhlersSineWaveIndicator
    {
        [Column("sine")]
        public decimal? Sine { get; set; }

        [Column("lead")]
        public decimal? LeadSine { get; set; }
    }
}

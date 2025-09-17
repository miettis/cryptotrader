using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Cycles
{
    [Owned]
    public class EhlersFourierSeriesAnalysis
    {
        [Column("wave")]
        public decimal? Wave { get; set; }

        [Column("roc")]
        public decimal? Roc { get; set; }
    }
}

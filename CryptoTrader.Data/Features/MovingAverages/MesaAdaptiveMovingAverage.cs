using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.MovingAverages
{
    [Owned]
    public class MesaAdaptiveMovingAverage
    {
        public static readonly double DefaultFastLimit = 0.5;
        public static readonly double DefaultSlowLimit = 0.05;

        [Column("mama")]
        public decimal? MAMA { get; set; }

        [Column("fama")]
        public decimal? FAMA { get; set; }
    }
}

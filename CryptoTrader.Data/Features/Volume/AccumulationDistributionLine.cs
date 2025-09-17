using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Volume
{
    [Owned]
    public class AccumulationDistributionLine
    {
        public static readonly int DefaultSmaPeriods = 12;

        [Column("mfm")]
        public decimal? MoneyFlowMultiplier { get; set; }

        [Column("mfv")]
        public decimal? MoneyFlowVolume { get; set; }

        [Column("adl")]
        public decimal? Adl { get; set; }

        [Column("sma")]
        public decimal? AdlSma { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Volume
{
    [Owned]
    public class ChaikinMoneyFlow
    {
        public static readonly int DefaultLookbackPeriods = 12;

        [Column("mfm")]
        public decimal? MoneyFlowMultiplier { get; set; }

        [Column("mfv")]
        public decimal? MoneyFlowVolume { get; set; }

        [Column("cmf")]
        public decimal? Cmf { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data.Features.Momentum
{
    [Owned]
    public class OnBalanceVolume
    {
        public static readonly int DefaultSmaPeriods = 12;

        [Column("obv")]
        public decimal? Obv { get; set; }

        [Column("sma")]
        public decimal? Sma { get; set; }
    }
}

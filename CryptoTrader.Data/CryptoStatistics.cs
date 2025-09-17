using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data
{
    [PrimaryKey(nameof(CryptoId), nameof(StartTime), nameof(EndTime))]
    public class CryptoStatistics
    {
        public static readonly int StatsCryptoId = 999;

        public int CryptoId { get; set; }
        public Crypto Crypto { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public CandlePartStatistics Candle { get; set; } = new CandlePartStatistics();
        public CandlePartStatistics Upper { get; set; } = new CandlePartStatistics();
        public CandlePartStatistics Body { get; set; } = new CandlePartStatistics();
        public CandlePartStatistics Lower { get; set; } = new CandlePartStatistics();
    }

    [Owned]
    public class CandlePartStatistics
    {
        public CandleLimits Length { get; set; } = new CandleLimits();
        public CandleLimits Proportion { get; set; } = new CandleLimits();
    }

    [Owned]
    public class CandleLimits
    {
        [Precision(30,28)]
        public decimal Mean { get; set; }
        [Precision(30,28)]
        public decimal Std { get; set; }
        [Precision(30,28)]
        public decimal Lim5 { get; set; }
        [Precision(30,28)]
        public decimal Lim10 { get; set; }
        [Precision(30,28)]
        public decimal Lim25 { get; set; }
        [Precision(30,28)]
        public decimal Lim50 { get; set; }
        [Precision(30,28)]
        public decimal Lim75 { get; set; }
        [Precision(30,28)]
        public decimal Lim90 { get; set; }
        [Precision(30,28)]
        public decimal Lim95 { get; set; }
    }
}

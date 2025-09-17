using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTrader.Data
{
    public class Crypto
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public required string Symbol { get; set; }

        [MaxLength(50)]
        public required string Name { get; set; }

        public int Rank { get; set; }

        public bool Trade { get; set; }

        public decimal? MaxPurchase { get; set; }

        public decimal? MaxTotal { get; set; }

        public DateTimeOffset? Followed { get; set; }

        public Timestamps Times { get; set; } = new Timestamps();

        public bool Active { get; set; }

        public List<Price> Prices { get; set; }

        public List<PricePrediction> Predictions { get; set; }
        public List<CryptoStatistics> Statistics { get; set; }
        public List<CryptoModel> Models { get; set; }
    }

    [Owned]
    public class Timestamps
    {
        [Column("start")]
        public DateTimeOffset Start { get; set; }

        [Column("start_data")]
        public DateTimeOffset StartData { get; set; }

        [Column("end_data")]
        public DateTimeOffset? EndData { get; set; }

        [Column("end_candle")]
        public DateTimeOffset? EndCandleSticks { get; set; }

        [Column("end_cycle")]
        public DateTimeOffset? EndCycle { get; set; }

        [Column("end_ma")]
        public DateTimeOffset? EndMA { get; set; }

        [Column("end_momentum")]
        public DateTimeOffset? EndMomentum { get; set; }

        [Column("end_peak")]
        public DateTimeOffset? EndPeak { get; set; }

        [Column("end_return")]
        public DateTimeOffset? EndReturn { get; set; }

        [Column("end_slope")]
        public DateTimeOffset? EndSlope { get; set; }

        [Column("end_trend")]
        public DateTimeOffset? EndTrend { get; set; }

        [Column("end_volatility")]
        public DateTimeOffset? EndVolatility { get; set; }

        [Column("end_volume")]
        public DateTimeOffset? EndVolume { get; set; }

        [Column("end_other")]
        public DateTimeOffset? EndOther { get; set; }

    }
}

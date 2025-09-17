using CryptoTrader.Data.Features.Volatility;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CryptoTrader.Data.Features
{
    public class PriceVolatilities : FeatureContainer
    {
        // Choppiness Index
        public static readonly int DefaultChopLoopbackPeriods = 12;

        // Ulcer Index
        public static readonly int DefaultUlcerLoopbackPeriods = 12;

        [JsonIgnore]
        public long Id { get; set; }

        [JsonIgnore]
        public Price Price { get; set; } = null!;

        [Column("atr12")]
        [JsonPropertyName("atr12")]
        public AverageTrueRange ATR12 { get; set; } = new AverageTrueRange();

        [Column("atr24")]
        [JsonPropertyName("atr24")]
        public AverageTrueRange ATR24 { get; set; } = new AverageTrueRange();

        [Column("atr168")]
        [JsonPropertyName("atr168")]
        public AverageTrueRange ATR168 { get; set; } = new AverageTrueRange();

        [Column("bb")]
        public BollingerBand BollingerBands { get; set; } = new BollingerBand();

        [Column("chop")]
        public decimal? ChoppinessIndex { get; set; }

        [Column("dc")]
        public DonchianChannel DonchianChannels { get; set; } = new DonchianChannel();

        [Column("hv")]
        public decimal? HistoricalVolatility { get; set; }

        [Column("kc")]
        public KeltnerChannel KeltnerChannels { get; set; } = new KeltnerChannel();

        [Column("sdc")]
        public StandardDeviationChannel StandardDeviationChannel { get; set; } = new StandardDeviationChannel();

        [Column("starc")]
        public StarcBand StarcBand { get; set; } = new StarcBand();

        [Column("ui")]
        public decimal? UlcerIndex { get; set; }

        [IgnoreDataCheck]
        [Column("vstop")]
        public VolatilityStop VolatilityStop { get; set; } = new VolatilityStop();
    }
}

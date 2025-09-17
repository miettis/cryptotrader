using CryptoTrader.Data.Features.MovingAverages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CryptoTrader.Data.Features
{
    public class PriceMovingAverages : FeatureContainer
    {
        public static readonly double DefaultAlmaOffset = 0.85;
        public static readonly double DefaultAlmaSigma = 6;

        [JsonIgnore]
        public long Id { get; set; }

        [JsonIgnore]
        public Price Price { get; set; }

        [Column("alma6")]
        [JsonPropertyName("alma6")]
        public decimal? ALMA6 { get; set; }

        [Column("alma12")]
        [JsonPropertyName("alma12")]
        public decimal? ALMA12 { get; set; }

        [Column("alma24")]
        [JsonPropertyName("alma24")]
        public decimal? ALMA24 { get; set; }

        [Column("alma168")]
        [JsonPropertyName("alma168")]
        public decimal? ALMA168 { get; set; }

        [Column("dema6")]
        [JsonPropertyName("dema6")]
        public decimal? DEMA6 { get; set; }

        [Column("dema12")]
        [JsonPropertyName("dema12")]
        public decimal? DEMA12 { get; set; }

        [Column("dema24")]
        [JsonPropertyName("dema24")]
        public decimal? DEMA24 { get; set; }

        [Column("dema168")]
        [JsonPropertyName("dema168")]
        public decimal? DEMA168 { get; set; }

        [Column("ema6")]
        [JsonPropertyName("ema6")]
        public decimal? EMA6 { get; set; }

        [Column("ema12")]
        [JsonPropertyName("ema12")]
        public decimal? EMA12 { get; set; }

        [Column("ema24")]
        [JsonPropertyName("ema24")]
        public decimal? EMA24 { get; set; }

        [Column("ema168")]
        [JsonPropertyName("ema168")]
        public decimal? EMA168 { get; set; }

        [Column("epma6")]
        [JsonPropertyName("epma6")]
        public decimal? EPMA6 { get; set; }

        [Column("epma12")]
        [JsonPropertyName("epma12")]
        public decimal? EPMA12 { get; set; }

        [Column("epma24")]
        [JsonPropertyName("epma24")]
        public decimal? EPMA24 { get; set; }

        [Column("epma168")]
        [JsonPropertyName("epma168")]
        public decimal? EPMA168 { get; set; }

        [Column("hma6")]
        [JsonPropertyName("hma6")]
        public decimal? HMA6 { get; set; }

        [Column("hma12")]
        [JsonPropertyName("hma12")]
        public decimal? HMA12 { get; set; }

        [Column("hma24")]
        [JsonPropertyName("hma24")]
        public decimal? HMA24 { get; set; }

        [Column("hma168")]
        [JsonPropertyName("hma168")]
        public decimal? HMA168 { get; set; }

        [Column("kama")]
        [JsonPropertyName("kama")]
        public KaufmansAdaptiveMovingAverage KAMA { get; set; } = new KaufmansAdaptiveMovingAverage();

        [Column("mama")]
        [JsonPropertyName("mama")]
        public MesaAdaptiveMovingAverage MAMA { get; set; } = new MesaAdaptiveMovingAverage();

        [Column("sma12")]
        [JsonPropertyName("sma12")]
        public SimpleMovingAverage SMA12 { get; set; } = new SimpleMovingAverage();

        [Column("sma24")]
        [JsonPropertyName("sma24")]
        public SimpleMovingAverage SMA24 { get; set; } = new SimpleMovingAverage();

        [Column("sma168")]
        [JsonPropertyName("sma168")]
        public SimpleMovingAverage SMA168 { get; set; } = new SimpleMovingAverage();

        [Column("smma6")]
        [JsonPropertyName("smma6")]
        public decimal? SMMA6 { get; set; }

        [Column("smma12")]
        [JsonPropertyName("smma12")]
        public decimal? SMMA12 { get; set; }

        [Column("smma24")]
        [JsonPropertyName("smma24")]
        public decimal? SMMA24 { get; set; }

        [Column("smma168")]
        [JsonPropertyName("smma168")]
        public decimal? SMMA168 { get; set; }

        [Column("t3_6")]
        [JsonPropertyName("t3_6")]
        public decimal? T3_6 { get; set; }

        [Column("t3_12")]
        [JsonPropertyName("t3_12")]
        public decimal? T3_12 { get; set; }

        [Column("t3_246")]
        [JsonPropertyName("t3_24")]
        public decimal? T3_24 { get; set; }

        [Column("t3_168")]
        [JsonPropertyName("t3_168")]
        public decimal? T3_168 { get; set; }

        [Column("tema6")]
        [JsonPropertyName("tema6")]
        public decimal? TEMA6 { get; set; }

        [Column("tema12")]
        [JsonPropertyName("tema12")]
        public decimal? TEMA12 { get; set; }

        [Column("tema24")]
        [JsonPropertyName("tema24")]
        public decimal? TEMA24 { get; set; }

        [Column("tema168")]
        [JsonPropertyName("tema168")]
        public decimal? TEMA168 { get; set; }

        [Column("wma6")]
        [JsonPropertyName("wma6")]
        public decimal? WMA6 { get; set; }

        [Column("wma12")]
        [JsonPropertyName("wma12")]
        public decimal? WMA12 { get; set; }

        [Column("wma24")]
        [JsonPropertyName("wma24")]
        public decimal? WMA24 { get; set; }

        [Column("wma168")]
        [JsonPropertyName("wma168")]
        public decimal? WMA168 { get; set; }

        [Column("vwma6")]
        [JsonPropertyName("vwma6")]
        public decimal? VWMA6 { get; set; }

        [Column("vwma12")]
        [JsonPropertyName("vwma12")]
        public decimal? VWMA12 { get; set; }

        [Column("vwma24")]
        [JsonPropertyName("vwma24")]
        public decimal? VWMA24 { get; set; }

        [Column("vwma168")]
        [JsonPropertyName("vwma168")]
        public decimal? VWMA168 { get; set; }
    }
}

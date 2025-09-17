using CryptoTrader.Data.Features.Misc;
using CryptoTrader.Data.Features.Momentum;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CryptoTrader.Data.Features
{
    public class PriceMomentums : FeatureContainer
    {
        // Balance of Power
        public static readonly int DefaultBopSmoothPeriods = 12;

        // Chande Momentum Oscillator
        public static readonly int DefaultCmoLookbackPeriods = 12;

        // Commodity Channel Index
        public static readonly int DefaultCciLookbackPeriods = 12;

        // Relative Strength Index
        public static readonly int DefaultRsiLookbackPeriods = 12;

        // Schaff Trend Cycle
        public static readonly int DefaultStcCyclePeriods = 6;
        public static readonly int DefaultStcFastPeriods = 12;
        public static readonly int DefaultStcSlowPeriods = 24;

        // Ultimate Oscillator
        public static readonly int DefaultUltimateShortPeriods = 6;
        public static readonly int DefaultUltimateMiddlePeriods = 12;
        public static readonly int DefaultUltimateLongPeriods = 24;

        // Williams %R
        public static readonly int DefaultWilliamsLookbackPeriods = 12;

        [JsonIgnore]
        public long Id { get; set; }

        [JsonIgnore]
        public Price Price { get; set; } = null!;

        [Column("apo")]
        public decimal? AbsolutePriceOscillator { get; set; }

        [Column("ao")]
        public AwesomeOscillator AwesomeOscillator { get; set; } = new AwesomeOscillator();

        [Column("bop")]
        public decimal? BalanceOfPower { get; set; }

        [Column("cmo")]
        public decimal? ChandeMomentumOscillator { get; set; }

        [Column("cci")]
        public decimal? CommodityChannelIndex { get; set; }

        [Column("csi")]
        public CommoditySelectionIndex CommoditySelectionIndex { get; set; } = new CommoditySelectionIndex();

        [Column("crsi")]
        public ConnorsRSI ConnorsRSI { get; set; } = new ConnorsRSI();

        [IgnoreDataCheck]
        [Column("dpo")]
        public DetrendedPriceOscillator DetrendedPriceOscillator { get; set; } = new DetrendedPriceOscillator();

        [Column("eri")]
        public ElderRayIndex ElderRayIndex { get; set; } = new ElderRayIndex();

        [Column("go")]
        public GatorOscillator GatorOscillator { get; set; } = new GatorOscillator();

        [Column("macd")]
        public MovingAverageConvergenceDivergence MACD { get; set; } = new MovingAverageConvergenceDivergence();

        [Column("obv")]
        public OnBalanceVolume OnBalanceVolume { get; set; } = new OnBalanceVolume();

        [Column("ppo")]
        public PercentagePriceOscillator PercentagePriceOscillator { get; set; } = new PercentagePriceOscillator();

        [Column("pmo")]
        public PriceMomentumOscillator PriceMomentumOscillator { get; set; } = new PriceMomentumOscillator();

        [Column("roc")]
        public RateOfChange RateOfChange { get; set; } = new RateOfChange();

        [Column("rsi")]
        public decimal? RelativeStrengthIndex { get; set; }

        [Column("rocwb")]
        public RocWithBands RocWithBands { get; set; } = new RocWithBands();

        [Column("stc")]
        public decimal? SchaffTrendCycle { get; set; }

        [Column("sfo")]
        public StochasticFastOscillator StochasticFastOscillator { get; set; } = new StochasticFastOscillator();

        [Column("smi")]
        public StochasticMomentumIndex StochasticMomentumIndex { get; set; } = new StochasticMomentumIndex();

        [Column("stoch")]
        public StochasticOscillator StochasticOscillator { get; set; } = new StochasticOscillator();

        [Column("stochrsi")]
        public StochasticRSI StochasticRSI { get; set; } = new StochasticRSI();

        [Column("trix")]
        public TripleEmaOscillator TRIX { get; set; } = new TripleEmaOscillator();

        [Column("tsi")]
        public TrueStrengthIndex TrueStrengthIndex { get; set; } = new TrueStrengthIndex();

        [Column("ultimate")]
        public decimal? UltimateOscillator { get; set; }

        [Column("williamsr")]
        public decimal? WilliamsR { get; set; }
    }
}

using CryptoTrader.Data.Features.Misc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CryptoTrader.Data.Features
{
    public class PriceOtherIndicators : FeatureContainer
    {
        // Hurst Exponent
        public static readonly int DefaultHurstExponentLoopbackPeriods = 24;

        [JsonIgnore]
        public long Id { get; set; }

        [JsonIgnore]
        public Price Price { get; set; } = null!;

        [Column("fcb")]
        public FractalChaosBand FractalChaosBands { get; set; } = new FractalChaosBand();

        [Column("he")]
        public decimal? HurstExponent { get; set; }

        [IgnoreDataCheck]
        [Column("pivot")]
        public Pivot Pivot { get; set; } = new Pivot();

        [Column("pp")]
        public PivotPoints PivotPoints { get; set; } = new PivotPoints();

        [IgnoreDataCheck]
        [Column("prs")]
        public PriceRelativeStrength PriceRelativeStrength { get; set; } = new PriceRelativeStrength();

        [Column("po")]
        public ProjectionOscillator ProjectionOscillator { get; set; } = new ProjectionOscillator();

        [Column("rpp")]
        public PivotPoints RollingPivotPoints { get; set; } = new PivotPoints();

        [IgnoreDataCheck]
        [Column("wf")]
        public WilliamsFractal WilliamsFractal { get; set; } = new WilliamsFractal();

        [Column("vhf")]
        public VerticalHorizontalFilter VerticalHorizontalFilter { get; set; } = new VerticalHorizontalFilter();
    }
}

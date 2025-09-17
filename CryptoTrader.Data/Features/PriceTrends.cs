using CryptoTrader.Data.Features.Characteristics;
using CryptoTrader.Data.Features.Trends;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CryptoTrader.Data.Features
{
    public class PriceTrends : FeatureContainer
    {
        [JsonIgnore]
        public long Id { get; set; }

        [JsonIgnore]
        public Price Price { get; set; } = null!;

        [IgnoreDataCheck]
        [Column("slope")]
        public SlopeHour Slope { get; set; } = new SlopeHour();

        [Column("aroon")]
        public Aroon Aroon { get; set; } = new Aroon();

        [Column("adx")]
        public AverageDirectionalIndex ADX { get; set; } = new AverageDirectionalIndex();

        [IgnoreDataCheck]
        [Column("atr")]
        public AtrTrailingStop AtrTrailingStop { get; set; } = new AtrTrailingStop();

        [Column("ce")]
        public ChandelierExit ChandelierExit { get; set; } = new ChandelierExit();

        [Column("hilb")]
        public HilbertTransformInstantaneousTrendLine HilbertTransform { get; set; } = new HilbertTransformInstantaneousTrendLine();

        [IgnoreDataCheck]
        [Column("ic")]
        public IchimokuCloud IchimokuCloud { get; set; } = new IchimokuCloud();

        [Column("mgd")]
        public McGinleyDynamic McGinleyDynamic { get; set; } = new McGinleyDynamic();

        [Column("mae")]
        public MovingAverageEnvelope MovingAverageEnvelope { get; set; } = new MovingAverageEnvelope();

        [Column("par")]
        public ParabolicSAR ParabolicSAR { get; set; } = new ParabolicSAR();

        [IgnoreDataCheck]
        [Column("st")]
        public SuperTrend SuperTrend { get; set; } = new SuperTrend();

        [Column("vi")]
        public VortexIndicator VortexIndicator { get; set; } = new VortexIndicator();

        [Column("wa")]
        public WilliamsAlligator WilliamsAlligator { get; set; } = new WilliamsAlligator();
    }
}

using CryptoTrader.Data.Features.Oscillators;
using CryptoTrader.Data.Features.Volume;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CryptoTrader.Data.Features
{
    public class PriceVolumes : FeatureContainer
    {
        public static readonly int DefaultForceIndexLookbackPeriods = 2;
        public static readonly int DefaultMoneyFlowIndexLookbackPeriods = 12;

        [JsonIgnore]
        public long Id { get; set; }

        [JsonIgnore]
        public Price Price { get; set; } = null!;

        [Column("adl")]
        public AccumulationDistributionLine AccumulationDistributionLine { get; set; } = new AccumulationDistributionLine();

        [Column("cmf")]
        public ChaikinMoneyFlow ChaikinMoneyFlow { get; set; } = new ChaikinMoneyFlow();

        [Column("co")]
        public ChaikinOscillator ChaikinOscillator { get; set; } = new ChaikinOscillator();

        [Column("fi")]
        public decimal? ForceIndex { get; set; }

        [Column("kvo")]
        public KlingerVolumeOscillator KlingerVolumeOscillator { get; set; } = new KlingerVolumeOscillator();

        [Column("mfi")]
        public decimal? MoneyFlowIndex { get; set; }

        [Column("pvo")]
        public PercentageVolumeOscillator PercentageVolumeOscillator { get;set; } = new PercentageVolumeOscillator();
    }
}

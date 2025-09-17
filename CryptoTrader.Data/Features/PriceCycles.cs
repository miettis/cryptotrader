using CryptoTrader.Data.Features.Cycles;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CryptoTrader.Data.Features
{
    public class PriceCycles : FeatureContainer
    {
        [JsonIgnore]
        public long Id { get; set; }

        [JsonIgnore]
        public Price Price { get; set; } = null!;

        [Column("eacc")]
        public EhlersAdaptiveCyberCycle EhlersAdaptiveCyberCycle { get; set; } = new EhlersAdaptiveCyberCycle();

        //[IgnoreDataCheck]
        //[Column("eacp")]
        //public decimal? EhlersAutoCorrelationPeriodogram { get; set; }

        [Column("ecfse")]
        public decimal? EhlersCombFilterSpectralEstimate { get; set; }

        [Column("ecc")]
        public decimal? EhlersCyberCycle { get; set; }

        [Column("eca")]
        public decimal? EhlersCycleAmplitude { get; set; }

        [Column("ecbpf")]
        public decimal? EhlersCycleBandPassFilter { get; set; }

        //[IgnoreDataCheck]
        //[Column("edftse")]
        //public decimal? EhlersDiscreteFourierTransformSpectralEstimate { get; set; }

        [Column("edddc")]
        public decimal? EhlersDualDifferentiatorDominantCycle { get; set; }

        [Column("ebsi")]
        public decimal? EhlersEvenBetterSineWaveIndicator { get; set; }

        [Column("efsa")]
        public EhlersFourierSeriesAnalysis EhlersFourierSeriesAnalysis { get; set; } = new EhlersFourierSeriesAnalysis();

        [Column("ehdc")]
        public decimal? EhlersHomodyneDominantCycle { get; set; }

        [Column("eipi")]
        public decimal? EhlersInstantaneousPhaseIndicator { get; set; }

        [Column("epadc")]
        public decimal? EhlersPhaseAccumulationDominantCycle { get; set; }

        [Column("esci")]
        public decimal? EhlersSimpleCycleIndicator { get; set; }

        [Column("eswi1")]
        public EhlersSineWaveIndicator EhlersSineWaveIndicatorV1 { get; set; } = new EhlersSineWaveIndicator();

        [Column("eswi2")]
        public EhlersSineWaveIndicator EhlersSineWaveIndicatorV2 { get; set; } = new EhlersSineWaveIndicator();

        [Column("esdfb")]
        public decimal? EhlersSpectrumDerivedFilterBank { get; set; }

        [Column("esi")]
        public decimal? EhlersSquelchIndicator { get; set; }

        [Column("escc")]
        public EhlersStochasticCyberCycle EhlersStochasticCyberCycle { get; set; } = new EhlersStochasticCyberCycle();

        [Column("ezcdc")]
        public decimal? EhlersZeroCrossingsDominantCycle { get; set; }

        [Column("glco")]
        public decimal? GroverLlorensCycleOscillator { get; set; }

        [Column("hcc")]
        public HurstCycleChannel HurstCycleChannel { get;set; } = new HurstCycleChannel();

        [Column("sc")]
        public decimal? SimpleCycle { get; set; }
    }
}

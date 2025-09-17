using System.ComponentModel.DataAnnotations.Schema;
using TechnicalAnalysis.Candles;

namespace CryptoTrader.Data.Features
{
    public class PriceCandleSticks : FeatureContainer
    {

        public long Id { get; set; }
        public Price Price { get; set; } = null!;

        /*
        [Column("enpl")]
        public bool EightNewPriceLines { get; set; }

        [Column("tnpl1")]
        public bool TenNewPriceLines { get; set; }

        [Column("tnpl2")]
        public bool TwelveNewPriceLines { get; set; }

        [Column("tnpl3")]
        public bool ThirteenNewPriceLines { get; set; }
        */


        [TaLibMapping("CDLABANDONEDBABY", CandleStickType.Bearish)]
        [Column("ab_bear")]
        public bool AbandonedBabyBearish { get; set; }

        [TaLibMapping("CDLABANDONEDBABY", CandleStickType.Bullish)]
        [Column("ab_bull")]
        public bool AbandonedBabyBullish { get; set; }

        [Column("ats")]
        public bool AboveTheStomach { get; set; }

        [TaLibMapping("CDLADVANCEBLOCK", CandleStickType.Undefined)]
        [Column("adv")]
        public bool AdvanceBlock { get; set; }

        [Column("bts")]
        public bool BelowTheStomach { get; set; }

        [TaLibMapping("CDLBELTHOLD", CandleStickType.Bearish)]
        [Column("bh_bear")]
        public bool BeltHoldBearish { get; set; }

        [TaLibMapping("CDLBELTHOLD", CandleStickType.Bullish)]
        [Column("bh_bull")]
        public bool BeltHoldBullish { get; set; }

        [TaLibMapping("CDLBREAKAWAY", CandleStickType.Bearish)]
        [Column("ba_bear")]
        public bool BreakawayBearish { get; set; }

        [TaLibMapping("CDLBREAKAWAY", CandleStickType.Bullish)]
        [Column("ba_bull")]
        public bool BreakawayBullish { get; set; }

        [Column("cb")]
        public bool CandleBlack { get; set; }

        [Column("csb")]
        public bool CandleShortBlack { get; set; }

        [Column("csw")]
        public bool CandleShortWhite { get; set; }

        [Column("cw")]
        public bool CandleWhite { get; set; }

        [TaLibMapping("CDLCONCEALBABYSWALL", CandleStickType.Undefined)]
        [Column("cbs")]
        public bool ConcealingBabySwallow { get; set; }

        [TaLibMapping("CDLCOUNTERATTACK", CandleStickType.Bearish)]
        [Column("ca_bear")]
        public bool CounterAttackBearish { get; set; }

        [TaLibMapping("CDLCOUNTERATTACK", CandleStickType.Bullish)]
        [Column("ca_bull")]
        public bool CounterAttackBullish { get; set; }

        [TaLibMapping("CDLDARKCLOUDCOVER", CandleStickType.Undefined)]
        [Column("dcc")]
        public bool DarkCloudCover { get; set; }

        [Column("del")]
        public bool Deliberation { get; set; }

        [TaLibMapping("CDLDOJI", CandleStickType.Undefined)]
        [Column("doji")]
        public bool Doji { get; set; }

        [TaLibMapping("CDLDRAGONFLYDOJI", CandleStickType.Undefined)]
        [Column("dd")]
        public bool DojiDragonfly { get; set; }

        [Column("dgd")]
        public bool DojiGappingDown { get; set; }

        [Column("dgu")]
        public bool DojiGappingUp { get; set; }

        [TaLibMapping("CDLGRAVESTONEDOJI", CandleStickType.Undefined)]
        [Column("dgs")]
        public bool DojiGravestone { get; set; }

        [TaLibMapping("CDLLONGLEGGEDDOJI", CandleStickType.Undefined)]
        [Column("dll")]
        public bool DojiLongLegged { get; set; }

        [Column("dn")]
        public bool DojiNorthern { get; set; }

        [Column("ds")]
        public bool DojiSouthern { get; set; }

        [TaLibMapping("CDLDOJISTAR", CandleStickType.Bearish)]
        [Column("ds_bear")]
        public bool DojiStarBearish { get; set; }

        [TaLibMapping("CDLDOJISTAR", CandleStickType.Bullish)]
        [Column("ds_bull")]
        public bool DojiStarBullish { get; set; }

        [Column("dsc")]
        public bool DojiStarCollapsing { get; set; }

        [TaLibMapping("CDLXSIDEGAP3METHODS", CandleStickType.Bearish)]
        [Column("dgtm")]
        public bool DownsideGapThreeMethods { get; set; }

        [TaLibMapping("CDLTASUKIGAP", CandleStickType.Bearish)]
        [Column("dtg")]
        public bool DownsideTasukiGap { get; set; }

        [TaLibMapping("CDLENGULFING", CandleStickType.Bearish)]
        [Column("eng_bear")]
        public bool EngulfingBearish { get; set; }

        [TaLibMapping("CDLENGULFING", CandleStickType.Bullish)]
        [Column("eng_bull")]
        public bool EngulfingBullish { get; set; }

        [TaLibMapping("CDLEVENINGDOJISTAR", CandleStickType.Undefined)]
        [Column("eds")]
        public bool EveningDojiStar { get; set; }

        [TaLibMapping("CDLEVENINGSTAR", CandleStickType.Undefined)]
        [Column("es")]
        public bool EveningStar { get; set; }

        [TaLibMapping("CDLRISEFALL3METHODS", CandleStickType.Bearish)]
        [Column("ftm")]
        public bool FallingThreeMethods { get; set; }

        [TaLibMapping("CDLHAMMER", CandleStickType.Undefined)]
        [Column("hammer")]
        public bool Hammer { get; set; }

        [TaLibMapping("CDLINVERTEDHAMMER", CandleStickType.Undefined)]
        [Column("inv")]
        public bool HammerInverted { get; set; }

        [TaLibMapping("CDLHANGINGMAN", CandleStickType.Undefined)]
        [Column("hm")]
        public bool HangingMan { get; set; }

        [TaLibMapping("CDLHARAMI", CandleStickType.Bearish)]
        [Column("ha_bear")]
        public bool HaramiBearish { get; set; }

        [TaLibMapping("CDLHARAMI", CandleStickType.Bullish)]
        [Column("ha_bull")]
        public bool HaramiBullish { get; set; }

        [TaLibMapping("CDLHARAMICROSS", CandleStickType.Bearish)]
        [Column("hc_bear")]
        public bool HaramiCrossBearish { get; set; }

        [TaLibMapping("CDLHARAMICROSS", CandleStickType.Bullish)]
        [Column("hc_bull")]
        public bool HaramiCrossBullish { get; set; }

        [TaLibMapping("CDLHIGHWAVE", CandleStickType.Bearish)]
        [Column("hw_bear")]
        public bool HighWaveBearish { get; set; }

        [TaLibMapping("CDLHIGHWAVE", CandleStickType.Bullish)]
        [Column("hw_bull")]
        public bool HighWaveBullish { get; set; }

        [TaLibMapping("CDLHIKKAKE", CandleStickType.Bearish)]
        [Column("hik_bear")]
        public bool HikkakeBearish { get; set; }

        [TaLibMapping("CDLHIKKAKE", CandleStickType.Bullish)]
        [Column("hik_bull")]
        public bool HikkakeBullish { get; set; }

        [TaLibMapping("CDLHIKKAKEMOD", CandleStickType.Bearish)]
        [Column("hmod_bear")]
        public bool HikkakeModBearish { get; set; }

        [TaLibMapping("CDLHIKKAKEMOD", CandleStickType.Bullish)]
        [Column("hmod_bull")]
        public bool HikkakeModBullish { get; set; }

        [TaLibMapping("CDLHOMINGPIGEON", CandleStickType.Undefined)]
        [Column("hp")]
        public bool HomingPidgeon { get; set; }

        [TaLibMapping("CDLIDENTICAL3CROWS", CandleStickType.Undefined)]
        [Column("itc")]
        public bool IdenticalThreeCrows { get; set; }

        [TaLibMapping("CDLINNECK", CandleStickType.Undefined)]
        [Column("in")]
        public bool InNeck { get; set; }

        [TaLibMapping("CDLKICKING", CandleStickType.Bearish)]
        [Column("ki_bear")]
        public bool KickingBearish { get; set; }

        [TaLibMapping("CDLKICKING", CandleStickType.Bullish)]
        [Column("ki_bull")]
        public bool KickingBullish { get; set; }

        [TaLibMapping("CDLKICKINGBYLENGTH", CandleStickType.Bearish)]
        [Column("kl_bear")]
        public bool KickingByLengthBearish { get; set; }

        [TaLibMapping("CDLKICKINGBYLENGTH", CandleStickType.Bullish)]
        [Column("kl_bull")]
        public bool KickingByLengthBullish { get; set; }

        [TaLibMapping("CDLLADDERBOTTOM", CandleStickType.Undefined)]
        [Column("lab")]
        public bool LadderBottom { get; set; }

        [Column("leb")]
        public bool LastEngulfingBottom { get; set; }

        [Column("let")]
        public bool LastEngulfingTop { get; set; }

        [TaLibMapping("CDLLONGLINE", CandleStickType.Bearish)]
        [Column("lbd")]
        public bool LongBlackDay { get; set; }

        [TaLibMapping("CDLLONGLINE", CandleStickType.Bullish)]
        [Column("lwd")]
        public bool LongWhiteDay { get; set; }

        [TaLibMapping("CDLMARUBOZU", CandleStickType.Bearish)]
        [Column("mb")]
        public bool MarubozuBlack { get; set; }

        [TaLibMapping("CDLCLOSINGMARUBOZU", CandleStickType.Bearish)]
        [Column("mcb")]
        public bool MarubozuClosingBlack { get; set; }

        [TaLibMapping("CDLCLOSINGMARUBOZU", CandleStickType.Bullish)]
        [Column("mcw")]
        public bool MarubozuClosingWhite { get; set; }

        [Column("mob")]
        public bool MarubozuOpeningBlack { get; set; }

        [Column("mow")]
        public bool MarubozuOpeningWhite { get; set; }

        [TaLibMapping("CDLMARUBOZU", CandleStickType.Bullish)]
        [Column("mw")]
        public bool MarubozuWhite { get; set; }

        [TaLibMapping("CDLMATCHINGLOW", CandleStickType.Undefined)]
        [Column("ml")]
        public bool MatchingLow { get; set; }

        [TaLibMapping("CDLMATHOLD", CandleStickType.Undefined)]
        [Column("math")]
        public bool MatHold { get; set; }

        [Column("ml_bear")]
        public bool MeetingLinesBearish { get; set; }

        [Column("ml_bull")]
        public bool MeetingLinesBullish { get; set; }

        [TaLibMapping("CDLMORNINGDOJISTAR", CandleStickType.Undefined)]
        [Column("mds")]
        public bool MorningDojiStar { get; set; }

        [TaLibMapping("CDLMORNINGSTAR", CandleStickType.Undefined)]
        [Column("ms")]
        public bool MorningStar { get; set; }

        [TaLibMapping("CDLONNECK", CandleStickType.Undefined)]
        [Column("on")]
        public bool OnNeck { get; set; }

        [TaLibMapping("CDLPIERCING", CandleStickType.Undefined)]
        [Column("pp")]
        public bool PiercingPattern { get; set; }

        [TaLibMapping("CDLRICKSHAWMAN", CandleStickType.Undefined)]
        [Column("rm")]
        public bool RickshawMan { get; set; }

        [TaLibMapping("CDLRISEFALL3METHODS", CandleStickType.Bullish)]
        [Column("rtm")]
        public bool RisingThreeMethods { get; set; }

        [TaLibMapping("CDLSEPARATINGLINES", CandleStickType.Bearish)]
        [Column("sel_bear")]
        public bool SeparatingLinesBearish { get; set; }

        [TaLibMapping("CDLSEPARATINGLINES", CandleStickType.Bullish)]
        [Column("sel_bull")]
        public bool SeparatingLinesBullish { get; set; }

        [TaLibMapping("CDLSHOOTINGSTAR", CandleStickType.Undefined)]
        [Column("ssoc")]
        public bool ShootingStarOneCandle { get; set; }

        [Column("sstc")]
        public bool ShootingStarTwoCandle { get; set; }

        [TaLibMapping("CDLSHORTLINE", CandleStickType.Bearish)]
        [Column("shl_bear")]
        public bool ShortLineBearish { get; set; }

        [TaLibMapping("CDLSHORTLINE", CandleStickType.Bullish)]
        [Column("shl_bull")]
        public bool ShortLineBullish { get; set; }

        [Column("sbwl_bear")]
        public bool SideBySideWhiteLinesBearish { get; set; }

        [Column("sbwl_bull")]
        public bool SideBySideWhiteLinesBullish { get; set; }

        [TaLibMapping("CDLSPINNINGTOP", CandleStickType.Bearish)]
        [Column("stb")]
        public bool SpinningTopBlack { get; set; }

        [TaLibMapping("CDLSPINNINGTOP", CandleStickType.Bullish)]
        [Column("stw")]
        public bool SpinningTopWhite { get; set; }

        [TaLibMapping("CDLSTALLEDPATTERN", CandleStickType.Undefined)]
        [Column("sp")]
        public bool StalledPattern { get; set; }

        [TaLibMapping("CDLSTICKSANDWICH", CandleStickType.Undefined)]
        [Column("ss")]
        public bool StickSandwich { get; set; }

        [TaLibMapping("CDLTAKURI", CandleStickType.Undefined)]
        [Column("tl")]
        public bool TakuriLine { get; set; }

        [TaLibMapping("CDL3BLACKCROWS", CandleStickType.Undefined)]
        [Column("tbc")]
        public bool ThreeBlackCrows { get; set; }

        [TaLibMapping("CDL3INSIDE", CandleStickType.Bearish)]
        [Column("tid")]
        public bool ThreeInsideDown { get; set; }

        [TaLibMapping("CDL3INSIDE", CandleStickType.Bullish)]
        [Column("tiu")]
        public bool ThreeInsideUp { get; set; }

        [TaLibMapping("CDL3LINESTRIKE", CandleStickType.Bearish)]
        [Column("tls_bear")]
        public bool ThreeLineStrikeBearish { get; set; }

        [TaLibMapping("CDL3LINESTRIKE", CandleStickType.Bullish)]
        [Column("tls_bull")]
        public bool ThreeLineStrikeBullish { get; set; }

        [TaLibMapping("CDL3OUTSIDE", CandleStickType.Bearish)]
        [Column("tod")]
        public bool ThreeOutsideDown { get; set; }

        [TaLibMapping("CDL3OUTSIDE", CandleStickType.Bullish)]
        [Column("tou")]
        public bool ThreeOutsideUp { get; set; }

        [TaLibMapping("CDL3STARSINSOUTH", CandleStickType.Undefined)]
        [Column("tsits")]
        public bool ThreeStarsInTheSouth { get; set; }

        [TaLibMapping("CDL3WHITESOLDIERS", CandleStickType.Undefined)]
        [Column("tws")]
        public bool ThreeWhiteSoldiers { get; set; }

        [TaLibMapping("CDLTHRUSTING", CandleStickType.Undefined)]
        [Column("thru")]
        public bool Thrusting { get; set; }

        [TaLibMapping("CDLTRISTAR", CandleStickType.Bearish)]
        [Column("ts_bear")]
        public bool TriStarBearish { get; set; }

        [TaLibMapping("CDLTRISTAR", CandleStickType.Bullish)]
        [Column("ts_bull")]
        public bool TriStarBullish { get; set; }

        [Column("twb")]
        public bool TweezersBottom { get; set; }

        [Column("twt")]
        public bool TweezersTop { get; set; }

        [Column("tbgc")]
        public bool TwoBlackGappingCandles { get; set; }

        [TaLibMapping("CDL2CROWS", CandleStickType.Undefined)]
        [Column("tc")]
        public bool TwoCrows { get; set; }

        [TaLibMapping("CDLUNIQUE3RIVER", CandleStickType.Undefined)]
        [Column("utrb")]
        public bool UniqueThreeRiverBottom { get; set; }

        [TaLibMapping("CDLGAPSIDESIDEWHITE", CandleStickType.Bearish)]
        [Column("ugswl_bear")]
        public bool UpsideGapSideBySideWhiteLineBearish { get; set; }

        [TaLibMapping("CDLGAPSIDESIDEWHITE", CandleStickType.Bullish)]
        [Column("ugswl_bull")]
        public bool UpsideGapSideBySideWhiteLineBullish { get; set; }

        [TaLibMapping("CDLXSIDEGAP3METHODS", CandleStickType.Bullish)]
        [Column("ugtm")]
        public bool UpsideGapThreeMethods { get; set; }

        [TaLibMapping("CDLUPSIDEGAP2CROWS", CandleStickType.Undefined)]
        [Column("ugc")]
        public bool UpsideGapTwoCrows { get; set; }

        [TaLibMapping("CDLTASUKIGAP", CandleStickType.Bullish)]
        [Column("utg")]
        public bool UpsideTasukiGap { get; set; }

        [Column("wif")]
        public bool WindowFalling { get; set; }

        [Column("wir")]
        public bool WindowRising { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class TaLibMappingAttribute : Attribute
    {
        public string FunctionName { get; set; }
        public CandleStickType Type { get; set; }

        public TaLibMappingAttribute(string functionName, CandleStickType type)
        {
            FunctionName = functionName;
            Type = type;
        }
    }
    public enum CandleStickType
    {
        Undefined,
        Bullish,
        Bearish
    }
}
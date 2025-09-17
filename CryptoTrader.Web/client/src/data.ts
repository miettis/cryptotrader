import { PriceHourResponse } from './api';

export enum Feature {
  // Moving Averages
  ALMA6 = 'ALMA6',
  ALMA12 = 'ALMA12',
  ALMA24 = 'ALMA24',
  ALMA168 = 'ALMA168',
  DEMA6 = 'DEMA6',
  DEMA12 = 'DEMA12',
  DEMA24 = 'DEMA24',
  DEMA168 = 'DEMA168',
  EMA6 = 'EMA6',
  EMA12 = 'EMA12',
  EMA24 = 'EMA24',
  EMA168 = 'EMA168',
  EPMA6 = 'EPMA6',
  EPMA12 = 'EPMA12',
  EPMA24 = 'EPMA24',
  EPMA168 = 'EPMA168',
  HMA6 = 'HMA6',
  HMA12 = 'HMA12',
  HMA24 = 'HMA24',
  HMA168 = 'HMA168',
  KAMA = 'KAMA',
  MAMA = 'MAMA',
  SMA12 = 'SMA12',
  SMA24 = 'SMA24',
  SMA168 = 'SMA168',
  SMMA6 = 'SMMA6',
  SMMA12 = 'SMMA12',
  SMMA24 = 'SMMA24',
  SMMA168 = 'SMMA168',
  T3_6 = 'T3_6',
  T3_12 = 'T3_12',
  T3_24 = 'T3_24',
  T3_168 = 'T3_168',
  TEMA6 = 'TEMA6',
  TEMA12 = 'TEMA12',
  TEMA24 = 'TEMA24',
  TEMA168 = 'TEMA168',
  VWMA6 = 'VWMA6',
  VWMA12 = 'VWMA12',
  VWMA24 = 'VWMA24',
  VWMA168 = 'VWMA168',
  WMA6 = 'WMA6',
  WMA12 = 'WMA12',
  WMA24 = 'WMA24',
  WMA168 = 'WMA168',

  // Trends
  Aroon = 'Aroon',
  AverageDirectionalIndex = 'AverageDirectionalIndex',
  AtrTrailingStop = 'AtrTrailingStop',
  ChandelierExit = 'ChandelierExit',
  HilbertTransform = 'HilbertTransform',
  IchimokuCloud = 'IchimokuCloud',
  McGinleyDynamic = 'McGinleyDynamic',
  MovingAverageEnvelope = 'MovingAverageEnvelope',
  ParabolicSAR = 'ParabolicSAR',
  SuperTrend = 'SuperTrend',
  VortexIndicator = 'VortexIndicator',
  WilliamsAlligator = 'WilliamsAlligator',

  // Cycles
  EhlersAdaptiveCyberCycle = 'EhlersAdaptiveCyberCycle',
  EhlersCombFilterSpectralEstimate = 'EhlersCombFilterSpectralEstimate',
  EhlersCyberCycle = 'EhlersCyberCycle',
  EhlersCycleAmplitude = 'EhlersCycleAmplitude',
  EhlersCycleBandPassFilter = 'EhlersCycleBandPassFilter',
  EhlersDualDifferentiatorDominantCycle = 'EhlersDualDifferentiatorDominantCycle',
  EhlersEvenBetterSineWaveIndicator = 'EhlersEvenBetterSineWaveIndicator',
  EhlersFourierSeriesAnalysis = 'EhlersFourierSeriesAnalysis',
  EhlersHomodyneDominantCycle = 'EhlersHomodyneDominantCycle',
  EhlersInstantaneousPhaseIndicator = 'EhlersInstantaneousPhaseIndicator',
  EhlersPhaseAccumulationDominantCycle = 'EhlersPhaseAccumulationDominantCycle',
  EhlersSimpleCycleIndicator = 'EhlersSimpleCycleIndicator',
  EhlersSineWaveIndicatorV1 = 'EhlersSineWaveIndicatorV1',
  EhlersSineWaveIndicatorV2 = 'EhlersSineWaveIndicatorV2',
  EhlersSpectrumDerivedFilterBank = 'EhlersSpectrumDerivedFilterBank',
  EhlersSquelchIndicator = 'EhlersSquelchIndicator',
  EhlersStochasticCyberCycle = 'EhlersStochasticCyberCycle',
  EhlersZeroCrossingsDominantCycle = 'EhlersZeroCrossingsDominantCycle',
  GroverLlorensCycleOscillator = 'GroverLlorensCycleOscillator',
  HurstCycleChannel = 'HurstCycleChannel',
  SimpleCycle = 'SimpleCycle',

  // Momentum
  AbsolutePriceOscillator = 'AbsolutePriceOscillator',
  AwesomeOscillator = 'AwesomeOscillator',
  BalanceOfPower = 'BalanceOfPower',
  ChandeMomentumOscillator = 'ChandeMomentumOscillator',
  CommodityChannelIndex = 'CommodityChannelIndex',
  CommoditySelectionIndex = 'CommoditySelectionIndex',
  ConnorsRSI = 'ConnorsRSI',
  DetrendedPriceOscillator = 'DetrendedPriceOscillator',
  ElderRayIndex = 'ElderRayIndex',
  GatorOscillator = 'GatorOscillator',
  MACD = 'MACD',
  OnBalanceVolume = 'OnBalanceVolume',
  PercentagePriceOscillator = 'PercentagePriceOscillator',
  PriceMomentumOscillator = 'PriceMomentumOscillator',
  RateOfChange = 'RateOfChange',
  RelativeStrengthIndex = 'RelativeStrengthIndex',
  RocWithBands = 'RocWithBands',
  SchaffTrendCycle = 'SchaffTrendCycle',
  StochasticFastOscillator = 'StochasticFastOscillator',
  StochasticMomentumIndex = 'StochasticMomentumIndex',
  StochasticOscillator = 'StochasticOscillator',
  StochasticRSI = 'StochasticRSI',
  TRIX = 'TRIX',
  TrueStrengthIndex = 'TrueStrengthIndex',
  UltimateOscillator = 'UltimateOscillator',
  WilliamsR = 'WilliamsR',

  // Volume
  AccumulationDistributionLine = 'AccumulationDistributionLine',
  ChaikinMoneyFlow = 'ChaikinMoneyFlow',
  ChaikinOscillator = 'ChaikinOscillator',
  ForceIndex = 'ForceIndex',
  KlingerVolumeOscillator = 'KlingerVolumeOscillator',
  MoneyFlowIndex = 'MoneyFlowIndex',
  PercentageVolumeOscillator = 'PercentageVolumeOscillator',

  // Volatility
  ATR12 = 'ATR12',
  ATR24 = 'ATR24',
  ATR168 = 'ATR168',
  BollingerBands = 'BollingerBands',
  ChoppinessIndex = 'ChoppinessIndex',
  DonchianChannels = 'DonchianChannels',
  HistoricalVolatility = 'HistoricalVolatility',
  KeltnerChannels = 'KeltnerChannels',
  StandardDeviationChannels = 'StandardDeviationChannels',
  StarcBand = 'StarcBand',
  UlcerIndex = 'UlcerIndex',
  VolatilityStop = 'VolatilityStop',

  // Other
  FractalChaosBands = 'FractalChaosBands',
  HurstExponent = 'HurstExponent',
  Pivot = 'Pivot',
  PivotPoints = 'PivotPoints',
  PriceRelativeStrength = 'PriceRelativeStrength',
  ProjectionOscillator = 'ProjectionOscillator',
  RollingPivotPoints = 'RollingPivotPoints',
  WilliamsFractal = 'WilliamsFractal',
  VerticalHorizontalFilter = 'VerticalHorizontalFilter',

  // Peak
  Peak = 'Peak',

  // Return
}
export interface ChartData {
  chart: 'price' | 'oscillator' | 'volume';
  axis: 1 | 2;
  name: string;
  type: string;
  min?: number;
  max?: number;
  data: [number, number][];
}

export function getMovingAverageFeatures(): Feature[] {
  return [
    Feature.ALMA6,
    Feature.ALMA12,
    Feature.ALMA24,
    Feature.ALMA168,
    Feature.DEMA6,
    Feature.DEMA12,
    Feature.DEMA24,
    Feature.DEMA168,
    Feature.EMA6,
    Feature.EMA12,
    Feature.EMA24,
    Feature.EMA168,
    Feature.EPMA6,
    Feature.EPMA12,
    Feature.EPMA24,
    Feature.EPMA168,
    Feature.HMA6,
    Feature.HMA12,
    Feature.HMA24,
    Feature.HMA168,
    Feature.KAMA,
    Feature.MAMA,
    Feature.SMA12,
    Feature.SMA24,
    Feature.SMA168,
    Feature.SMMA6,
    Feature.SMMA12,
    Feature.SMMA24,
    Feature.SMMA168,
    Feature.T3_6,
    Feature.T3_12,
    Feature.T3_24,
    Feature.T3_168,
    Feature.TEMA6,
    Feature.TEMA12,
    Feature.TEMA24,
    Feature.TEMA168,
    Feature.VWMA6,
    Feature.VWMA12,
    Feature.VWMA24,
    Feature.VWMA168,
    Feature.WMA6,
    Feature.WMA12,
    Feature.WMA24,
    Feature.WMA168,
  ];
}
export function getTrendFeatures(): Feature[] {
  return [
    Feature.Aroon,
    Feature.AverageDirectionalIndex,
    Feature.AtrTrailingStop,
    Feature.ChandelierExit,
    Feature.HilbertTransform,
    Feature.IchimokuCloud,
    Feature.McGinleyDynamic,
    Feature.MovingAverageEnvelope,
    Feature.ParabolicSAR,
    Feature.SuperTrend,
    Feature.VortexIndicator,
    Feature.WilliamsAlligator,
  ];
}
export function getCycleFeatures(): Feature[] {
  return [
    Feature.EhlersAdaptiveCyberCycle,
    Feature.EhlersCombFilterSpectralEstimate,
    Feature.EhlersCyberCycle,
    Feature.EhlersCycleAmplitude,
    Feature.EhlersCycleBandPassFilter,
    Feature.EhlersDualDifferentiatorDominantCycle,
    Feature.EhlersEvenBetterSineWaveIndicator,
    Feature.EhlersFourierSeriesAnalysis,
    Feature.EhlersHomodyneDominantCycle,
    Feature.EhlersInstantaneousPhaseIndicator,
    Feature.EhlersPhaseAccumulationDominantCycle,
    Feature.EhlersSimpleCycleIndicator,
    Feature.EhlersSineWaveIndicatorV1,
    Feature.EhlersSineWaveIndicatorV2,
    Feature.EhlersSpectrumDerivedFilterBank,
    Feature.EhlersSquelchIndicator,
    Feature.EhlersStochasticCyberCycle,
    Feature.EhlersZeroCrossingsDominantCycle,
    Feature.GroverLlorensCycleOscillator,
    Feature.HurstCycleChannel,
    Feature.SimpleCycle,
  ];
}
export function getMomentumFeatures(): Feature[] {
  return [
    Feature.AbsolutePriceOscillator,
    Feature.AwesomeOscillator,
    Feature.BalanceOfPower,
    Feature.ChandeMomentumOscillator,
    Feature.CommodityChannelIndex,
    Feature.CommoditySelectionIndex,
    Feature.ConnorsRSI,
    Feature.DetrendedPriceOscillator,
    Feature.ElderRayIndex,
    Feature.GatorOscillator,
    Feature.MACD,
    Feature.OnBalanceVolume,
    Feature.PercentagePriceOscillator,
    Feature.PriceMomentumOscillator,
    Feature.RateOfChange,
    Feature.RelativeStrengthIndex,
    Feature.RocWithBands,
    Feature.SchaffTrendCycle,
    Feature.StochasticFastOscillator,
    Feature.StochasticMomentumIndex,
    Feature.StochasticOscillator,
    Feature.StochasticRSI,
    Feature.TRIX,
    Feature.TrueStrengthIndex,
    Feature.UltimateOscillator,
    Feature.WilliamsR,
  ];
}
export function getVolumeFeatures(): Feature[] {
  return [
    Feature.AccumulationDistributionLine,
    Feature.ChaikinMoneyFlow,
    Feature.ChaikinOscillator,
    Feature.ForceIndex,
    Feature.KlingerVolumeOscillator,
    Feature.MoneyFlowIndex,
    Feature.PercentageVolumeOscillator,
  ];
}
export function getVolatilityFeatures(): Feature[] {
  return [
    Feature.ATR12,
    Feature.ATR24,
    Feature.ATR168,
    Feature.BollingerBands,
    Feature.ChoppinessIndex,
    Feature.DonchianChannels,
    Feature.HistoricalVolatility,
    Feature.KeltnerChannels,
    Feature.StandardDeviationChannels,
    Feature.StarcBand,
    Feature.UlcerIndex,
    Feature.VolatilityStop,
  ];
}
export function getOtherFeatures(): Feature[] {
  return [
    Feature.FractalChaosBands,
    Feature.HurstExponent,
    Feature.Pivot,
    Feature.PivotPoints,
    Feature.PriceRelativeStrength,
    Feature.ProjectionOscillator,
    Feature.RollingPivotPoints,
    Feature.WilliamsFractal,
    Feature.VerticalHorizontalFilter,
  ];
}

export function getAllFeatures(): Feature[] {
  return [
    ...getMovingAverageFeatures(),
    ...getTrendFeatures(),
    ...getCycleFeatures(),
    ...getMomentumFeatures(),
    ...getVolumeFeatures(),
    ...getVolatilityFeatures(),
    ...getOtherFeatures(),
  ];
}

export function getFeatureData(
  prices: PriceHourResponse[],
  feature: Feature
): ChartData[] {
  let features: ChartData[] = [];

  switch (feature) {
    // MOVING AVERAGES
    case Feature.ALMA6:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'ALMA 6',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.alma6 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.ALMA12:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'ALMA 12',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.alma12 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.ALMA24:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'ALMA 24',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.alma24 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.ALMA168:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'ALMA 168',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.alma168 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.DEMA6:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'DEMA 6',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.dema6 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.DEMA12:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'DEMA 12',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.dema12 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.DEMA24:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'DEMA 24',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.dema24 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.DEMA168:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'DEMA 168',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.dema168 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EMA6:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'EMA 6',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.ema6 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EMA12:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'EMA 12',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.ema12 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EMA24:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'EMA 24',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.ema24 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EMA168:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'EMA 168',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.ema168 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EPMA6:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'EPMA 6',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.epma6 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EPMA12:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'EPMA 12',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.epma12 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EPMA24:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'EPMA 24',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.epma24 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EPMA168:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'EPMA 168',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.epma168 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.HMA6:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'HMA 6',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.hma6 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.HMA12:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'HMA 12',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.hma12 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.HMA24:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'HMA 24',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.hma24 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.HMA168:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'HMA 168',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.hma168 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.KAMA:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'KAMA',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.kama?.kama ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'ER',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.kama?.er ?? 0,
          ]),
        },
      ];
      break;
    case Feature.MAMA:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'MAMA',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.mama?.mama ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'FAMA',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.mama?.fama ?? 0,
          ]),
        },
      ];
      break;
    case Feature.SMA12:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'SMA 12',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.sma12?.sma ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 2,
          name: 'SMA 12 MAD',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.sma12?.mad ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 2,
          name: 'SMA 12 MSE',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.sma12?.mse ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 2,
          name: 'SMA 12 MAPE',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.sma12?.mape ?? 0,
          ]),
        },
      ];
      break;
    case Feature.SMA24:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'SMA 24',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.sma24?.sma ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 2,
          name: 'SMA 24 MAD',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.sma24?.mad ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 2,
          name: 'SMA 24 MSE',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.sma24?.mse ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 2,
          name: 'SMA 24 MAPE',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.sma24?.mape ?? 0,
          ]),
        },
      ];
      break;
    case Feature.SMA168:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'SMA 168',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.sma168?.sma ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 2,
          name: 'SMA 168 MAD',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.sma168?.mad ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 2,
          name: 'SMA 168 MSE',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.sma168?.mse ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 2,
          name: 'SMA 168 MAPE',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.sma168?.mape ?? 0,
          ]),
        },
      ];
      break;
    case Feature.SMMA6:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'SMMA 6',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.smma6 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.SMMA12:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'SMMA 12',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.smma12 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.SMMA24:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'SMMA 24',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.smma24 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.SMMA168:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'SMMA 168',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.smma168 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.T3_6:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'T3 6',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.t3_6 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.T3_12:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'T3 12',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.t3_12 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.T3_24:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'T3 24',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.t3_24 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.T3_168:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'T3 168',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.t3_168 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.TEMA6:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'TEMA 6',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.tema6 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.TEMA12:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'TEMA 12',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.tema12 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.TEMA24:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'TEMA 24',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.tema24 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.TEMA168:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'TEMA 168',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.tema168 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.VWMA6:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'VWMA 6',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.vwma6 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.VWMA12:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'VWMA 12',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.vwma12 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.VWMA24:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'VWMA 24',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.vwma24 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.VWMA168:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'VWMA 168',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.vwma168 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.WMA6:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'WMA 6',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.wma6 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.WMA12:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'WMA 12',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.wma12 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.WMA24:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'WMA 24',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.wma24 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.WMA168:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'WMA 168',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.ma?.wma168 ?? 0,
          ]),
        },
      ];
      break;

    // TRENDS
    case Feature.Aroon:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Aroon Up',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.aroon?.up ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Aroon Down',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.aroon?.down ?? 0,
          ]),
        },
      ];
      break;
    case Feature.AverageDirectionalIndex:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'ADX',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.adx?.adx ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'ADXR',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.adx?.adxr ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'MDI',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.adx?.mdi ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'PDI',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.adx?.pdi ?? 0,
          ]),
        },
      ];
      break;
    case Feature.AtrTrailingStop:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'ATR Trailing Stop Buy Stop',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.atrTrailingStop?.buyStop ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'ATR Trailing Stop Sell Stop',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.atrTrailingStop?.sellStop ?? 0,
          ]),
        },
      ];
      break;
    case Feature.ChandelierExit:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Chandelier Exit Long',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.chandelierExit?.long ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Chandelier Exit Short',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.chandelierExit?.short ?? 0,
          ]),
        },
      ];
      break;
    case Feature.HilbertTransform:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Instantaneous Trendline',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.hilbertTransform?.trendLine ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Smooth',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.hilbertTransform?.smoothPrice ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Dominant Cycle Period',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.hilbertTransform?.dominantCyclePeriods ?? 0,
          ]),
        },
      ];
      break;
    case Feature.IchimokuCloud:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Signal',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.ichimokuCloud?.signal ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Base',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.ichimokuCloud?.base ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Leading Span A',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.ichimokuCloud?.leadingA ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Leading Span B',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.ichimokuCloud?.leadingB ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Lagging Span',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.ichimokuCloud?.lagging ?? 0,
          ]),
        },
      ];
      break;
    case Feature.McGinleyDynamic:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'McGinley Dynamic 6',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.mcGinleyDynamic?.mcGinleyDynamic6 ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'McGinley Dynamic 12',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.mcGinleyDynamic?.mcGinleyDynamic12 ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'McGinley Dynamic 24',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.mcGinleyDynamic?.mcGinleyDynamic24 ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'McGinley Dynamic 168',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.mcGinleyDynamic?.mcGinleyDynamic168 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.MovingAverageEnvelope:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Moving Average Envelope Upper',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.movingAverageEnvelope?.upper ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Moving Average Envelope Lower',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.movingAverageEnvelope?.lower ?? 0,
          ]),
        },
      ];
      break;
    case Feature.ParabolicSAR:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Parabolic SAR',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.parabolicSAR?.sar ?? 0,
          ]),
        },
      ];
      break;
    case Feature.SuperTrend:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Super Trend Upper',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.superTrend?.upper ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Super Trend Lower',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.superTrend?.lower ?? 0,
          ]),
        },
      ];
      break;
    case Feature.VortexIndicator:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Vortex Indicator VI+',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.vortexIndicator?.pvi ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Vortex Indicator VI-',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.vortexIndicator?.nvi ?? 0,
          ]),
        },
      ];
      break;
    case Feature.WilliamsAlligator:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Williams Alligator Jaw',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.williamsAlligator?.jaw ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Williams Alligator Teeth',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.williamsAlligator?.teeth ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Williams Alligator Lips',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.trend?.williamsAlligator?.lips ?? 0,
          ]),
        },
      ];
      break;

    // CYCLES
    case Feature.EhlersAdaptiveCyberCycle:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Adaptive Cyber Cycle',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersAdaptiveCyberCycle?.cycle ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 2,
          name: 'Ehlers Adaptive Cyber Cycle Period',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersAdaptiveCyberCycle?.period ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EhlersCombFilterSpectralEstimate:
      features = [
        {
          chart: 'oscillator',
          axis: 2,
          name: 'Ehlers Comb Filter Spectral Estimate',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersCombFilterSpectralEstimate ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EhlersCyberCycle:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Cyber Cycle',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersCyberCycle ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EhlersCycleAmplitude:
      features = [
        {
          chart: 'oscillator',
          axis: 2,
          name: 'Ehlers Cycle Amplitude',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersCycleAmplitude ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EhlersCycleBandPassFilter:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Cycle Band Pass Filter',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersCycleBandPassFilter ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EhlersDualDifferentiatorDominantCycle:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Dual Differentiator Dominant Cycle',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersDualDifferentiatorDominantCycle ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EhlersEvenBetterSineWaveIndicator:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Even Better Sine Wave Indicator',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersEvenBetterSineWaveIndicator ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EhlersFourierSeriesAnalysis:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Fourier Series Analysis ROC',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersFourierSeriesAnalysis?.roc ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Fourier Series Analysis Wave',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersFourierSeriesAnalysis?.wave ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EhlersHomodyneDominantCycle:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Homodyne Dominant Cycle',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersHomodyneDominantCycle ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EhlersInstantaneousPhaseIndicator:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Instantaneous Phase Indicator',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersInstantaneousPhaseIndicator ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EhlersPhaseAccumulationDominantCycle:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Phase Accumulation Dominant Cycle',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersPhaseAccumulationDominantCycle ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EhlersSimpleCycleIndicator:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Simple Cycle Indicator',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersSimpleCycleIndicator ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EhlersSineWaveIndicatorV1:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Sine Wave Indicator V1 Sine',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersSineWaveIndicatorV1?.sine ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Sine Wave Indicator V1 Lead',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersSineWaveIndicatorV1?.leadSine ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EhlersSineWaveIndicatorV2:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Sine Wave Indicator V2 Sine',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersSineWaveIndicatorV2?.sine ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Sine Wave Indicator V2 Lead',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersSineWaveIndicatorV2?.leadSine ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EhlersSpectrumDerivedFilterBank:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Spectrum Derived Filter Bank',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersSpectrumDerivedFilterBank ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EhlersSquelchIndicator:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Squelch Indicator',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersSquelchIndicator ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EhlersStochasticCyberCycle:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Stochastic Cyber Cycle Signal',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersStochasticCyberCycle?.signal ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Stochastic Cyber Cycle Cycle',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersStochasticCyberCycle?.cycle ?? 0,
          ]),
        },
      ];
      break;
    case Feature.EhlersZeroCrossingsDominantCycle:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ehlers Zero Crossings Dominant Cycle',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.ehlersZeroCrossingsDominantCycle ?? 0,
          ]),
        },
      ];
      break;
    case Feature.GroverLlorensCycleOscillator:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Grover Llorens Cycle Oscillator',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.groverLlorensCycleOscillator ?? 0,
          ]),
        },
      ];
      break;
    case Feature.HurstCycleChannel:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Hurst Cycle Channel Fast',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.hurstCycleChannel?.fastUpperBand ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Hurst Cycle Channel Fast',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.hurstCycleChannel?.fastLowerBand ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Hurst Cycle Channel Fast',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.hurstCycleChannel?.fastMiddleBand ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Hurst Cycle Channel Slow',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.hurstCycleChannel?.slowUpperBand ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Hurst Cycle Channel Slow',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.hurstCycleChannel?.slowLowerBand ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Hurst Cycle Channel Slow',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.hurstCycleChannel?.slowMiddleBand ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 2,
          name: 'Hurst Cycle Channel O MED',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.hurstCycleChannel?.oMed ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 2,
          name: 'Hurst Cycle Channel O SHORT',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.hurstCycleChannel?.oShort ?? 0,
          ]),
        },
      ];
      break;
    case Feature.SimpleCycle:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Simple Cycle',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.cycles?.simpleCycle ?? 0,
          ]),
        },
      ];
      break;

    // MOMENTUM
    case Feature.AbsolutePriceOscillator:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Absolute Price Oscillator',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.absolutePriceOscillator ?? 0,
          ]),
        },
      ];
      break;
    case Feature.AwesomeOscillator:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Awesome Oscillator',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.awesomeOscillator?.oscillator ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 2,
          name: 'Awesome Oscillator Normalized',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.awesomeOscillator?.normalized ?? 0,
          ]),
        },
      ];
      break;
    case Feature.BalanceOfPower:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Balance of Power',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.balanceOfPower ?? 0,
          ]),
        },
      ];
      break;
    case Feature.ChandeMomentumOscillator:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Chande Momentum Oscillator',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.chandeMomentumOscillator ?? 0,
          ]),
        },
      ];
      break;
    case Feature.CommodityChannelIndex:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Commodity Channel Index',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.commodityChannelIndex ?? 0,
          ]),
        },
      ];
      break;
    case Feature.CommoditySelectionIndex:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Commodity Selection Index Signal',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.commoditySelectionIndex?.signal ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Commodity Selection Index CSI',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.commoditySelectionIndex?.csi ?? 0,
          ]),
        },
      ];
      break;
    case Feature.ConnorsRSI:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Connors RSI',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.connorsRSI?.connorsRsi ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 2,
          name: 'Connors RSI %Rank',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.connorsRSI?.percentRank ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Connors RSI RSI',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.connorsRSI?.rsi ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Connors RSI Streak',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.connorsRSI?.rsiStreak ?? 0,
          ]),
        },
      ];
      break;
    case Feature.DetrendedPriceOscillator:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Detrended Price Oscillator',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.detrendedPriceOscillator?.dpo ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Detrended Price Oscillator SMA',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.detrendedPriceOscillator?.sma ?? 0,
          ]),
        },
      ];
      break;
    case Feature.ElderRayIndex:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Elder Ray Bull Power',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.elderRayIndex?.bull ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Elder Ray Bear Power',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.elderRayIndex?.bear ?? 0,
          ]),
        },
      ];
      break;
    case Feature.GatorOscillator:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Gator Oscillator Upper',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.gatorOscillator?.upper ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Gator Oscillator Lower',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.gatorOscillator?.lower ?? 0,
          ]),
        },
      ];
      break;
    case Feature.MACD:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'MACD Line',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.macd?.macd ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Signal Line',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.macd?.signal ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Histogram',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.macd?.histogram ?? 0,
          ]),
        },
      ];
      break;
    case Feature.OnBalanceVolume:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'On Balance Volume',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.onBalanceVolume?.obv ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'On Balance Volume SMA',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.onBalanceVolume?.sma ?? 0,
          ]),
        },
      ];
      break;
    case Feature.PercentagePriceOscillator:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Percentage Price Oscillator',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.percentagePriceOscillator?.ppo ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Signal Line',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.percentagePriceOscillator?.signal ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Histogram',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.percentagePriceOscillator?.histogram ?? 0,
          ]),
        },
      ];
      break;
    case Feature.PriceMomentumOscillator:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Price Momentum Oscillator',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.priceMomentumOscillator?.pmo ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Price Momentum Oscillator Signal',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.priceMomentumOscillator?.signal ?? 0,
          ]),
        },
      ];
      break;
    case Feature.RateOfChange:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Rate of Change',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.rateOfChange?.roc ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Rate of Change SMA',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.rateOfChange?.sma ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Rate of Change Momentum',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.rateOfChange?.momentum ?? 0,
          ]),
        },
      ];
      break;
    case Feature.RelativeStrengthIndex:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Relative Strength Index',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.relativeStrengthIndex ?? 0,
          ]),
        },
      ];
      break;
    case Feature.RocWithBands:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'ROC with Bands Upper',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.rocWithBands?.upper ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'ROC with Bands Lower',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.rocWithBands?.lower ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'ROC',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.rocWithBands?.roc ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'EMA',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.rocWithBands?.ema ?? 0,
          ]),
        },
      ];
      break;
    case Feature.SchaffTrendCycle:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Schaff Trend Cycle',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.schaffTrendCycle ?? 0,
          ]),
        },
      ];
      break;
    case Feature.StochasticFastOscillator:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Stochastic Fast Oscillator',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.stochasticFastOscillator?.sfo ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Signal',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.stochasticFastOscillator?.signal ?? 0,
          ]),
        },
      ];
      break;
    case Feature.StochasticMomentumIndex:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Stochastic Momentum Index',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.stochasticMomentumIndex?.smi ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Signal',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.stochasticMomentumIndex?.signal ?? 0,
          ]),
        },
      ];
      break;
    case Feature.StochasticOscillator:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Stochastic Oscillator',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.stochasticOscillator?.oscillator ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Signal',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.stochasticOscillator?.signal ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: '%J',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.stochasticOscillator?.percentJ ?? 0,
          ]),
        },
      ];
      break;
    case Feature.StochasticRSI:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Stochastic RSI',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.stochasticRSI?.stochRsi ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Signal',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.stochasticRSI?.signal ?? 0,
          ]),
        },
      ];
      break;
    case Feature.TRIX:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'TRIX',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.trix?.trix ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Signal',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.trix?.signal ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'EMA',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.trix?.emA3 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.TrueStrengthIndex:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'True Strength Index',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.trueStrengthIndex?.tsi ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Signal',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.trueStrengthIndex?.signal ?? 0,
          ]),
        },
      ];
      break;
    case Feature.UltimateOscillator:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ultimate Oscillator',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.ultimateOscillator ?? 0,
          ]),
        },
      ];
      break;
    case Feature.WilliamsR:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Williams %R',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.momentum?.williamsR ?? 0,
          ]),
        },
      ];
      break;

    // VOLUME
    case Feature.AccumulationDistributionLine:
      features = [
        {
          chart: 'volume',
          axis: 1,
          name: 'Accumulation Distribution Line',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volumes?.accumulationDistributionLine?.adl ?? 0,
          ]),
        },
        {
          chart: 'volume',
          axis: 1,
          name: 'SMA',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volumes?.accumulationDistributionLine?.adlSma ?? 0,
          ]),
        },
        {
          chart: 'volume',
          axis: 2,
          name: 'Multiplier',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volumes?.accumulationDistributionLine?.moneyFlowMultiplier ??
              0,
          ]),
        },
        {
          chart: 'volume',
          axis: 1,
          name: 'Volume',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volumes?.accumulationDistributionLine?.moneyFlowVolume ?? 0,
          ]),
        },
      ];
      break;
    case Feature.ChaikinMoneyFlow:
      features = [
        {
          chart: 'volume',
          axis: 2,
          name: 'Chaikin Money Flow',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volumes?.chaikinMoneyFlow?.cmf ?? 0,
          ]),
        },
        {
          chart: 'volume',
          axis: 2,
          name: 'Multiplies',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volumes?.chaikinMoneyFlow?.moneyFlowMultiplier ?? 0,
          ]),
        },
        {
          chart: 'volume',
          axis: 1,
          name: 'Volume',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volumes?.chaikinMoneyFlow?.moneyFlowVolume ?? 0,
          ]),
        },
      ];
      break;
    case Feature.ChaikinOscillator:
      features = [
        {
          chart: 'volume',
          axis: 1,
          name: 'Chaikin Oscillator',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volumes?.chaikinOscillator?.oscillator ?? 0,
          ]),
        },
        {
          chart: 'volume',
          axis: 1,
          name: 'ADL',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volumes?.chaikinOscillator?.adl ?? 0,
          ]),
        },
        {
          chart: 'volume',
          axis: 2,
          name: 'Multiplier',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volumes?.chaikinOscillator?.moneyFlowMultiplier ?? 0,
          ]),
        },
        {
          chart: 'volume',
          axis: 1,
          name: 'Volume',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volumes?.chaikinOscillator?.moneyFlowVolume ?? 0,
          ]),
        },
      ];
      break;
    case Feature.ForceIndex:
      features = [
        {
          chart: 'volume',
          axis: 1,
          name: 'Force Index',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volumes?.forceIndex ?? 0,
          ]),
        },
      ];
      break;
    case Feature.KlingerVolumeOscillator:
      features = [
        {
          chart: 'volume',
          axis: 1,
          name: 'Klinger Volume Oscillator',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volumes?.klingerVolumeOscillator?.oscillator ?? 0,
          ]),
        },
        {
          chart: 'volume',
          axis: 1,
          name: 'Signal',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volumes?.klingerVolumeOscillator?.signal ?? 0,
          ]),
        },
      ];
      break;
    case Feature.MoneyFlowIndex:
      features = [
        {
          chart: 'volume',
          axis: 2,
          name: 'Money Flow Index',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volumes?.moneyFlowIndex ?? 0,
          ]),
        },
      ];
      break;
    case Feature.PercentageVolumeOscillator:
      features = [
        {
          chart: 'volume',
          axis: 2,
          name: 'Percentage Volume Oscillator',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volumes?.percentageVolumeOscillator?.pvo ?? 0,
          ]),
        },
        {
          chart: 'volume',
          axis: 2,
          name: 'Signal',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volumes?.percentageVolumeOscillator?.signal ?? 0,
          ]),
        },
        {
          chart: 'volume',
          axis: 2,
          name: 'Histogram',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volumes?.percentageVolumeOscillator?.histogram ?? 0,
          ]),
        },
      ];
      break;

    // VOLATILITY
    case Feature.ATR12:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'ATR 12',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.atr12?.atr ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'TR 12',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.atr12?.tr ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 2,
          name: 'ATR %',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.atr12?.atrp ?? 0,
          ]),
        },
      ];
      break;
    case Feature.ATR24:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'ATR 24',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.atr24?.atr ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'TR 12',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.atr24?.tr ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 2,
          name: 'ATR %',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.atr24?.atrp ?? 0,
          ]),
        },
      ];
      break;
    case Feature.ATR168:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'ATR 168',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.atr168?.atr ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'TR 12',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.atr168?.tr ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 2,
          name: 'ATR %',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.atr168?.atrp ?? 0,
          ]),
        },
      ];
      break;
    case Feature.BollingerBands:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Bollinger Bands Upper',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.bollingerBands?.upper ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Bollinger Bands Lower',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.bollingerBands?.lower ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: '%P',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.bollingerBands?.percentB ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Z score',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.bollingerBands?.zScore ?? 0,
          ]),
        },
      ];
      break;
    case Feature.ChoppinessIndex:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Choppiness Index',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.choppinessIndex ?? 0,
          ]),
        },
      ];
      break;
    case Feature.DonchianChannels:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Donchian Channels Upper',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.donchianChannels?.upper ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Donchian Channels Lower',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.donchianChannels?.lower ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Donchian Channels Center',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.donchianChannels?.center ?? 0,
          ]),
        },
      ];
      break;
    case Feature.HistoricalVolatility:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Historical Volatility',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.historicalVolatility ?? 0,
          ]),
        },
      ];
      break;
    case Feature.KeltnerChannels:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Keltner Channels Upper',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.keltnerChannels?.upper ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Keltner Channels Lower',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.keltnerChannels?.lower ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Keltner Channels Center',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.keltnerChannels?.center ?? 0,
          ]),
        },
      ];
      break;
    case Feature.StandardDeviationChannels:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Standard Deviation Channel Upper',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.standardDeviationChannel?.upper ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Standard Deviation Channel Lower',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.standardDeviationChannel?.lower ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Standard Deviation Channel Center',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.standardDeviationChannel?.center ?? 0,
          ]),
        },
      ];
      break;
    case Feature.StarcBand:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Starc Band Upper',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.starcBand?.upper ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Starc Band Lower',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.starcBand?.lower ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Starc Band Center',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.starcBand?.center ?? 0,
          ]),
        },
      ];
      break;
    case Feature.UlcerIndex:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Ulcer Index',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.ulcerIndex ?? 0,
          ]),
        },
      ];
      break;
    case Feature.VolatilityStop:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Volatility Stop Upper',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.volatilityStop?.upper ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Volatility Stop Lower',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.volatility?.volatilityStop?.lower ?? 0,
          ]),
        },
      ];
      break;

    // OTHER
    case Feature.FractalChaosBands:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Fractal Chaos Band Upper',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.fractalChaosBands?.upper ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Fractal Chaos Band Lower',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.fractalChaosBands?.lower ?? 0,
          ]),
        },
      ];
      break;
    case Feature.HurstExponent:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Hurst Exponent',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.hurstExponent ?? 0,
          ]),
        },
      ];
      break;
    case Feature.Pivot:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Pivot Point High',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.pivot?.highLine ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Pivot Point Low',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.pivot?.lowLine ?? 0,
          ]),
        },
        // highPoint/lowPoint
      ];
      break;
    case Feature.PivotPoints:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Pivot Points Resistance 1',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.pivotPoints?.r1 ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Pivot Points Support 1',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.pivotPoints?.s1 ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Pivot Points Resistance 2',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.pivotPoints?.r2 ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Pivot Points Support 2',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.pivotPoints?.s2 ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Pivot Points Resistance 3',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.pivotPoints?.r3 ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Pivot Points Support 3',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.pivotPoints?.s3 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.PriceRelativeStrength:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Price Relative Strength',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.priceRelativeStrength?.prs ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'SMA',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.priceRelativeStrength?.sma ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: '%',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.priceRelativeStrength?.percent ?? 0,
          ]),
        },
      ];
      break;
    case Feature.ProjectionOscillator:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Projection Oscillator',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.projectionOscillator?.pbo ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Projection Oscillator Signal',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.projectionOscillator?.signal ?? 0,
          ]),
        },
      ];
      break;
    case Feature.RollingPivotPoints:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Pivot Points Resistance 1',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.rollingPivotPoints?.r1 ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Pivot Points Support 1',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.rollingPivotPoints?.s1 ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Pivot Points Resistance 2',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.rollingPivotPoints?.r2 ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Pivot Points Support 2',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.rollingPivotPoints?.s2 ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Pivot Points Resistance 3',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.rollingPivotPoints?.r3 ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Pivot Points Support 3',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.rollingPivotPoints?.s3 ?? 0,
          ]),
        },
      ];
      break;
    case Feature.WilliamsFractal:
      features = [
        {
          chart: 'price',
          axis: 1,
          name: 'Williams Fractal Bear',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.williamsFractal?.fractalBear ?? 0,
          ]),
        },
        {
          chart: 'price',
          axis: 1,
          name: 'Williams Fractal Bull',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.williamsFractal?.fractalBull ?? 0,
          ]),
        },
      ];
      break;
    case Feature.VerticalHorizontalFilter:
      features = [
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Vertical Horizontal Filter',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.verticalHorizontalFilter?.vhf ?? 0,
          ]),
        },
        {
          chart: 'oscillator',
          axis: 1,
          name: 'Vertical Horizontal Filter Signal',
          type: 'line',
          data: prices.map((price) => [
            price.timeOpen?.getTime() ?? 0,
            price.otherIndicators?.verticalHorizontalFilter?.signal ?? 0,
          ]),
        },
      ];
      break;
  }

  features.forEach((feature) => {
    let min = 0;
    let max = 0;

    // remove 0 values
    feature.data = feature.data.filter((data) => data[1] !== 0);

    feature.data.forEach((data) => {
      if (data[1] > max) {
        max = data[1];
      }
      if (data[1] < min) {
        min = data[1];
      }
    });
    feature.min = min;
    feature.max = max;
  });

  return features;
}

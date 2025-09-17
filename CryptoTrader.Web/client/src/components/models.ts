export interface Todo {
  id: number;
  content: string;
}

export interface Meta {
  totalCount: number;
}

export interface Asset {
  symbol: string;
  amount: number;
}

export interface PriceHour {
  cryptoId: number;
  symbol: string;
  timestamp: number;
  open: number;
  high: number;
  low: number;
  close: number;
  volume: number;
  quoteVolume: number;
  trades: number;
  buyVolume: number;
  butQuoteVolume: number;
  marketCap: number;
  avg: {
    ohlc: number;
    hlc: number;
    hl: number;
    oc: number;
  };
  ma: {
    h24: number;
    h24Std: number;
    h48: number;
    h48Std: number;
    h168: number;
    h168Std: number;
  };
}

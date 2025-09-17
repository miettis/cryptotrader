using CryptoTrader.Data.Features.CandleSticks;
using CryptoTrader.Data.Features.Cycles;
using CryptoTrader.Data.Features.Misc;
using CryptoTrader.Data.Features.Momentum;
using CryptoTrader.Data.Features.MovingAverages;
using CryptoTrader.Data.Features.Oscillators;
using CryptoTrader.Data.Features.Trends;
using CryptoTrader.Data.Features.Volatility;
using CryptoTrader.Data.Features.Volume;
using CryptoTrader.Data.Services;
using Microsoft.EntityFrameworkCore;
using OoplesFinance.StockIndicators;
using OoplesFinance.StockIndicators.Models;
using Skender.Stock.Indicators;
using System.Linq.Expressions;
using System.Reflection;

namespace CryptoTrader.Data.Features
{
    public class FeatureCalculation
    {
        private readonly IDbContextFactory<BinanceContext> _contextFactory;
        private readonly PythonService _predictionService;
        public FeatureCalculation(IDbContextFactory<BinanceContext> contextFactory, PythonService predictionService)
        {
            _contextFactory = contextFactory;
            _predictionService = predictionService;
        }

        public void UpdateCandleSticks(string symbol, DateOnly start, DateOnly end)
        {
            var historyHours = 24 * 30;
            var context = GetContext();
            var prices = GetPrices(context, symbol, start, end, historyHours, x => x.CandleSticks);
            UpdateCandleSticks(prices, new DateTimeOffset(start, TimeOnly.MinValue, TimeSpan.Zero)).Wait();

            var crypto = context.Cryptos.First(x => x.Symbol == symbol);
            UpdateEndTime(prices, x => x.CandleSticks, (time) => crypto.Times.EndCandleSticks = time);

            context.SaveChanges();
        }
        public void UpdateCandleSticks2(string symbol, DateOnly start, DateOnly end)
        {
            var historyHours = 24 * 30;
            var context = GetContext();
            var prices = GetPrices(context, symbol, start, end, historyHours, x => x.CandleSticks, x => x.Trend);
            var stats = context.CryptoStatistics.Where(x => x.Crypto.Symbol == symbol).OrderByDescending(x => x.EndTime).FirstOrDefault();
            UpdateCandleSticks2(prices, new DateTimeOffset(start, TimeOnly.MinValue, TimeSpan.Zero), stats).Wait();

            //context.SaveChanges();
        }
        public void UpdateCycles(string symbol, DateOnly start, DateOnly end)
        {
            var historyHours = 24 * 30;
            var context = GetContext();
            var prices = GetPrices(context, symbol, start, end, historyHours, x => x.Cycles);
            UpdateCycles(prices, new DateTimeOffset(start, TimeOnly.MinValue, TimeSpan.Zero));

            var crypto = context.Cryptos.First(x => x.Symbol == symbol);
            UpdateEndTime(prices, x => x.Cycles, (time) => crypto.Times.EndCycle = time);

            context.SaveChanges();
        }
        public void UpdateOtherIndicators(string symbol, DateOnly start, DateOnly end)
        {
            var historyHours = 24 * 30;
            var context = GetContext();
            var prices = GetPrices(context, symbol, start, end, historyHours, x => x.OtherIndicators);
            UpdateOtherIndicators(prices, new DateTimeOffset(start, TimeOnly.MinValue, TimeSpan.Zero));

            var crypto = context.Cryptos.First(x => x.Symbol == symbol);
            UpdateEndTime(prices, x => x.OtherIndicators, (time) => crypto.Times.EndOther = time);

            context.SaveChanges();
        }
        public void UpdateMomentum(string symbol, DateOnly start, DateOnly end)
        {
            var historyHours = 24 * 30;
            var context = GetContext();
            var prices = GetPrices(context, symbol, start, end, historyHours, x => x.Momentum);
            UpdateMomentum(prices, new DateTimeOffset(start, TimeOnly.MinValue, TimeSpan.Zero));

            var crypto = context.Cryptos.First(x => x.Symbol == symbol);
            UpdateEndTime(prices, x => x.Momentum, (time) => crypto.Times.EndMomentum = time);

            context.SaveChanges();
        }
        public void UpdateMovingAverages(string symbol, DateOnly start, DateOnly end)
        {
            var historyHours = 24 * 30;
            var context = GetContext();
            var prices = GetPrices(context, symbol, start, end, historyHours, x => x.MA);
            UpdateMovingAverages(prices, new DateTimeOffset(start, TimeOnly.MinValue, TimeSpan.Zero));

            var crypto = context.Cryptos.First(x => x.Symbol == symbol);
            UpdateEndTime(prices, x => x.MA, (time) => crypto.Times.EndMA = time);

            context.SaveChanges();
        }
        public void UpdatePeaks(string symbol, DateOnly start, DateOnly end)
        {
            var historyHours = 24 * 30;
            var context = GetContext();
            var prices = GetPrices(context, symbol, start, end, historyHours, x => x.Peak);
            UpdatePeaks(prices, new DateTimeOffset(start, TimeOnly.MinValue, TimeSpan.Zero), new DateTimeOffset(end, TimeOnly.MinValue, TimeSpan.Zero));

            var crypto = context.Cryptos.First(x => x.Symbol == symbol);
            UpdateEndTime(prices, x => x.Peak, (time) => crypto.Times.EndPeak = time);

            context.SaveChanges();
        }
        public void UpdateReturns(string symbol, DateOnly start, DateOnly end)
        {
            var historyHours = 24 * 30;
            var context = GetContext();
            var prices = GetPrices(context, symbol, start, end, historyHours, x => x.Return);
            UpdateReturns(prices);

            var crypto = context.Cryptos.First(x => x.Symbol == symbol);
            UpdateEndTime(prices, x => x.Return, (time) => crypto.Times.EndReturn = time);

            context.SaveChanges();
        }
        public void UpdateSlopes(string symbol, DateOnly start, DateOnly end)
        {
            var historyHours = 24 * 30;
            var context = GetContext();
            var prices = GetPrices(context, symbol, start, end, historyHours, x => x.Trend);
            UpdateSlopes(prices, new DateTimeOffset(start, TimeOnly.MinValue, TimeSpan.Zero), new DateTimeOffset(end, TimeOnly.MaxValue, TimeSpan.Zero)).Wait();

            var crypto = context.Cryptos.First(x => x.Symbol == symbol);
            var time = prices.Last(x => x.Trend?.Slope.High24.HasValue ?? false)?.TimeOpen;
            if (time != null && (crypto.Times.EndSlope == null || crypto.Times.EndSlope.Value < time))
            {
                crypto.Times.EndSlope = time;
            }

            context.SaveChanges();
        }
        public void UpdateTrends(string symbol, DateOnly start, DateOnly end)
        {
            var historyHours = 24 * 30;
            var context = GetContext();
            var prices = GetPrices(context, symbol, start, end, historyHours, x => x.Trend);
            UpdateTrends(prices, new DateTimeOffset(start, TimeOnly.MinValue, TimeSpan.Zero));

            var crypto = context.Cryptos.First(x => x.Symbol == symbol);
            UpdateEndTime(prices, x => x.Trend, (time) => crypto.Times.EndTrend = time);

            context.SaveChanges();
        }
        public void UpdateVolatilities(string symbol, DateOnly start, DateOnly end)
        {
            var historyHours = 24 * 30;
            var context = GetContext();
            var prices = GetPrices(context, symbol, start, end, historyHours, x => x.Volatility);
            UpdateVolatilities(prices, new DateTimeOffset(start, TimeOnly.MinValue, TimeSpan.Zero));

            var crypto = context.Cryptos.First(x => x.Symbol == symbol);
            UpdateEndTime(prices, x => x.Volatility, (time) => crypto.Times.EndVolatility = time);

            context.SaveChanges();
        }
        public void UpdateVolumes(string symbol, DateOnly start, DateOnly end)
        {
            var historyHours = 24 * 30;
            var context = GetContext();
            var prices = GetPrices(context, symbol, start, end, historyHours, x => x.Volumes);
            UpdateVolumes(prices, new DateTimeOffset(start, TimeOnly.MinValue, TimeSpan.Zero));

            var crypto = context.Cryptos.First(x => x.Symbol == symbol);
            UpdateEndTime(prices, x => x.Volumes, (time) => crypto.Times.EndVolume = time);

            context.SaveChanges();
        }


        public async Task UpdateCandleSticks(Price[] prices, DateTimeOffset start)
        {
            var bullishMapping = new Dictionary<string, PropertyInfo>();
            var bearishMapping = new Dictionary<string, PropertyInfo>();
            var neutralMapping = new Dictionary<string, PropertyInfo>();
            foreach(var property in typeof(PriceCandleSticks).GetProperties())
            {
                var taLibAttr = property.GetCustomAttribute<TaLibMappingAttribute>();
                if (taLibAttr == null)
                {
                    continue;
                }
                if(taLibAttr.Type == CandleStickType.Bullish)
                {
                    bullishMapping[taLibAttr.FunctionName.ToUpper()] = property;
                }
                else if(taLibAttr.Type == CandleStickType.Bearish)
                {
                    bearishMapping[taLibAttr.FunctionName.ToUpper()] =  property;
                }
                else
                {
                    neutralMapping[taLibAttr.FunctionName.ToUpper()] = property;
                }
            }
            ;
            var response = await _predictionService.GetCandleSticks(prices);
            var found = new Dictionary<string, int>();
            foreach(var matches in response)
            {
                var pattern = matches.Pattern.ToUpper();
                if(!bullishMapping.ContainsKey(pattern) && !bearishMapping.ContainsKey(pattern) && !neutralMapping.ContainsKey(pattern))
                {
                    await Console.Out.WriteLineAsync($"Unknown pattern {pattern}");
                }
                for (var i = 0; i < prices.Length; i++)
                {
                    var price = prices[i];
                    if (price.TimeOpen < start)
                    {
                        continue;
                    }
                    price.CandleSticks = price.CandleSticks ?? new PriceCandleSticks();
                    PropertyInfo prop = null;
                    if (matches.Result[i] > 0 && bullishMapping.TryGetValue(pattern, out prop))
                    {
                        prop.SetValue(price.CandleSticks, true);
                    }
                    else if (matches.Result[i] > 0 && neutralMapping.TryGetValue(pattern, out prop))
                    {
                        prop.SetValue(price.CandleSticks, true);
                    }
                    else if (matches.Result[i] < 0 && bearishMapping.TryGetValue(pattern, out prop))
                    {
                        prop.SetValue(price.CandleSticks, true);
                    }
                    else if (matches.Result[i] < 0 && neutralMapping.TryGetValue(pattern, out prop))
                    {
                        prop.SetValue(price.CandleSticks, true);
                    }

                    if (prop != null)
                    {
                        if (!found.ContainsKey(prop.Name))
                        {
                            found.Add(prop.Name, 0);
                        }
                        found[prop.Name]++;
                    }
                }
            }
            /*
            foreach(var kvp in found)
            {
                Console.WriteLine($"{kvp.Key.PadRight(50)} {kvp.Value}");
            }
            */
            /*
            var open = prices.Select(x => x.Open).ToArray();
            var high = prices.Select(x => x.High).ToArray();
            var low = prices.Select(x => x.Low).ToArray();
            var close = prices.Select(x => x.Close).ToArray();
            var methods = typeof(TACandle).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.IsStatic && x.ReturnType == typeof(CandleIndicatorResult))
                .ToArray();
            foreach(var method in methods)
            {
                try
                {
                    var decimalMethod = method.MakeGenericMethod(typeof(decimal));
                    var result = decimalMethod.Invoke(null, [0, prices.Length, open, high, low, close]) as CandleIndicatorResult;

                    for (var i = 0; i < prices.Length; i++)
                    {
                        var price = prices[i];
                        if (price.TimeOpen < start)
                        {
                            continue;
                        }
                        price.CandleSticks = price.CandleSticks ?? new PriceCandleSticks();

                        if (result.Integers[i] > 0 && bullishMapping.TryGetValue(method.Name, out var prop))
                        {
                            prop.SetValue(price.CandleSticks, true);
                        }
                        else if (result.Integers[i] > 0 && neutralMapping.TryGetValue(method.Name, out prop))
                        {
                            prop.SetValue(price.CandleSticks, true);
                        }
                        else if (result.Integers[i] < 0 && bearishMapping.TryGetValue(method.Name, out prop))
                        {
                            prop.SetValue(price.CandleSticks, true);
                        }
                        else if (result.Integers[i] < 0 && neutralMapping.TryGetValue(method.Name, out prop))
                        {
                            prop.SetValue(price.CandleSticks, true);
                        }
                    }
                }
                catch
                {
                    ;
                }
            }
            */
        }
        public async Task UpdateCandleSticks2(Price[] prices, DateTimeOffset start, CryptoStatistics stats)
        {
            var mapping = typeof(PriceCandleSticks).GetProperties()
                .Where(x => x.PropertyType == typeof(bool))
                .ToDictionary(x => x.Name);

            var cs = new CandleStick(stats);
            var result = cs.GetAllPatterns(prices);
            for(var i = 0; i < prices.Length; i++)
            {
                var current = prices[i];
                if (current.TimeOpen < start)
                {
                    continue;
                }

                var existing = mapping.Where(x => (bool)x.Value.GetValue(current.CandleSticks)).Select(x => Enum.Parse<CandleStickPattern>(x.Key, true)).ToArray();
                if(existing.Length == 0 && result[i].Length == 0)
                {
                    continue;
                }

                foreach(var newValue in result[i])
                {
                    if (!existing.Contains(newValue))
                    {
                        // new match
                        ;
                    }
                }
                foreach(var existingValue in existing)
                {
                    if (!result[i].Contains(existingValue))
                    {
                        // no longer a match
                        ;
                    }
                }

                foreach(var match in result[i])
                {
                    if(mapping.TryGetValue(match.ToString().ToUpper(), out var prop))
                    {
                        var existingValue = (bool)prop.GetValue(current.CandleSticks);
                        if(!existingValue)
                        {
                            continue;
                        }
                    }
                }
            }
        }
        public static void UpdateCycles(Price[] prices, DateTimeOffset start)
        {
            var open = prices.Select(x => (double)x.Open).ToArray();
            var high = prices.Select(x => (double)x.High).ToArray();
            var low = prices.Select(x => (double)x.Low).ToArray();
            var close = prices.Select(x => (double)x.Close).ToArray();
            var volume = prices.Select(x => (double)x.Volume).ToArray();
            var time = prices.Select(x => x.TimeOpen.DateTime).ToArray();


            var results = new StockData(open, high, low, close, volume, time).CalculateEhlersAdaptiveCyberCycle();
            var eacc = results.OutputValues["Eacc"];
            var period = results.OutputValues["Period"];

            //
            //results = new StockData(open, high, low, close, volume, time).CalculateEhlersAutoCorrelationPeriodogram();
            //var eacp = results.OutputValues["Eacp"];

            results = new StockData(open, high, low, close, volume, time).CalculateEhlersCombFilterSpectralEstimate();
            var ecfse = results.OutputValues["Ecfse"];

            results = new StockData(open, high, low, close, volume, time).CalculateEhlersCyberCycle();
            var ecc = results.OutputValues["Ecc"];

            results = new StockData(open, high, low, close, volume, time).CalculateEhlersCycleAmplitude();
            var eca = results.OutputValues["Eca"];

            results = new StockData(open, high, low, close, volume, time).CalculateEhlersCycleBandPassFilter();
            var ecbpf = results.OutputValues["Ecbpf"];

            //
            //results = new StockData(open, high, low, close, volume, time).CalculateEhlersDiscreteFourierTransformSpectralEstimate();
            //var edftse = results.OutputValues["Edftse"];

            results = new StockData(open, high, low, close, volume, time).CalculateEhlersDualDifferentiatorDominantCycle();
            var edddc = results.OutputValues["Edddc"];

            results = new StockData(open, high, low, close, volume, time).CalculateEhlersEvenBetterSineWaveIndicator();
            var ebsi = results.OutputValues["Ebsi"];

            results = new StockData(open, high, low, close, volume, time).CalculateEhlersFourierSeriesAnalysis();
            var wave = results.OutputValues["Wave"];
            var roc = results.OutputValues["Roc"];

            //
            results = new StockData(open, high, low, close, volume, time).CalculateEhlersHomodyneDominantCycle();
            var ehdc = results.OutputValues["Ehdc"];

            //
            results = new StockData(open, high, low, close, volume, time).CalculateEhlersInstantaneousPhaseIndicator();
            var eipi = results.OutputValues["Eipi"];
            //
            results = new StockData(open, high, low, close, volume, time).CalculateEhlersPhaseAccumulationDominantCycle();
            var epadc = results.OutputValues["Epadc"];

            results = new StockData(open, high, low, close, volume, time).CalculateEhlersSimpleCycleIndicator();
            var esci = results.OutputValues["Esci"];

            results = new StockData(open, high, low, close, volume, time).CalculateEhlersSineWaveIndicatorV1();
            var sine1 = results.OutputValues["Sine"];
            var lead1 = results.OutputValues["LeadSine"];


            results = new StockData(open, high, low, close, volume, time).CalculateEhlersSineWaveIndicatorV2();
            var sine2 = results.OutputValues["Sine"];
            var lead2 = results.OutputValues["LeadSine"];

            results = new StockData(open, high, low, close, volume, time).CalculateEhlersSpectrumDerivedFilterBank();
            var esdfb = results.OutputValues["Esdfb"];

            results = new StockData(open, high, low, close, volume, time).CalculateEhlersSquelchIndicator();
            var esi = results.OutputValues["Esi"];

            results = new StockData(open, high, low, close, volume, time).CalculateEhlersStochasticCyberCycle();
            var escc = results.OutputValues["Escc"];
            var signal = results.OutputValues["Signal"];

            results = new StockData(open, high, low, close, volume, time).CalculateEhlersZeroCrossingsDominantCycle();
            var ezcdc = results.OutputValues["Ezcdc"];

            results = new StockData(open, high, low, close, volume, time).CalculateGroverLlorensCycleOscillator();
            var glco = results.OutputValues["Glco"];


            results = new StockData(open, high, low, close, volume, time).CalculateHurstCycleChannel();
            var fub = results.OutputValues["FastUpperBand"];
            var sub = results.OutputValues["SlowUpperBand"];
            var fmb = results.OutputValues["FastMiddleBand"];
            var smb = results.OutputValues["SlowMiddleBand"];
            var flb = results.OutputValues["FastLowerBand"];
            var slb = results.OutputValues["SlowLowerBand"];
            var omed = results.OutputValues["OMed"];
            var oshort = results.OutputValues["OShort"];

            results = new StockData(open, high, low, close, volume, time).CalculateSimpleCycle();
            var sc = results.OutputValues["Sc"];

            for (var i = 0; i < prices.Length; i++)
            {
                var price = prices[i];
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Cycles = price.Cycles ?? new PriceCycles();

                price.Cycles.EhlersAdaptiveCyberCycle = new EhlersAdaptiveCyberCycle
                {
                    Cycle = (decimal)eacc[i],
                    Period = (decimal)period[i]
                };
                //price.Cycles.EhlersAutoCorrelationPeriodogram = (decimal)eacp[i];
                price.Cycles.EhlersCombFilterSpectralEstimate = (decimal)ecfse[i];
                price.Cycles.EhlersCyberCycle = (decimal)ecc[i];
                price.Cycles.EhlersCycleAmplitude = (decimal)eca[i];
                price.Cycles.EhlersCycleBandPassFilter = (decimal)ecbpf[i];
                //price.Cycles.EhlersDiscreteFourierTransformSpectralEstimate = (decimal)edftse[i];
                price.Cycles.EhlersDualDifferentiatorDominantCycle = (decimal)edddc[i];
                price.Cycles.EhlersEvenBetterSineWaveIndicator = (decimal)ebsi[i];
                price.Cycles.EhlersFourierSeriesAnalysis = new EhlersFourierSeriesAnalysis
                {
                    Wave = (decimal)wave[i],
                    Roc = (decimal)roc[i]
                };
                price.Cycles.EhlersHomodyneDominantCycle = (decimal)ehdc[i];
                price.Cycles.EhlersInstantaneousPhaseIndicator = (decimal)eipi[i];
                price.Cycles.EhlersPhaseAccumulationDominantCycle = (decimal)epadc[i];
                price.Cycles.EhlersSimpleCycleIndicator = (decimal)esci[i];
                price.Cycles.EhlersSineWaveIndicatorV1 = new EhlersSineWaveIndicator
                {
                    Sine = (decimal)sine1[i],
                    LeadSine = (decimal)lead1[i]
                };
                price.Cycles.EhlersSineWaveIndicatorV2 = new EhlersSineWaveIndicator
                {
                    Sine = (decimal)sine2[i],
                    LeadSine = (decimal)lead2[i]
                };
                price.Cycles.EhlersSpectrumDerivedFilterBank = (decimal)esdfb[i];
                price.Cycles.EhlersSquelchIndicator = (decimal)esi[i];
                price.Cycles.EhlersStochasticCyberCycle = new EhlersStochasticCyberCycle
                {
                    Cycle = (decimal)escc[i],
                    Signal = (decimal)signal[i]
                };
                price.Cycles.EhlersZeroCrossingsDominantCycle = (decimal)ezcdc[i];
                price.Cycles.GroverLlorensCycleOscillator = (decimal)glco[i];
                price.Cycles.HurstCycleChannel = new HurstCycleChannel
                {
                    FastUpperBand = (decimal)fub[i],
                    SlowUpperBand = (decimal)sub[i],
                    FastMiddleBand = (decimal)fmb[i],
                    SlowMiddleBand = (decimal)smb[i],
                    FastLowerBand = (decimal)flb[i],
                    SlowLowerBand = (decimal)slb[i],
                    OMed = (decimal)omed[i],
                    OShort = (decimal)oshort[i]
                };
                price.Cycles.SimpleCycle = (decimal)sc[i];

            }

        }
        public static void UpdateMomentum(Price[] prices, DateTimeOffset start)
        {
            foreach (var price in prices)
            {
                if (price.Momentum == null)
                {
                    price.Momentum = new PriceMomentums();
                }
            }

            // Awesome Oscillator
            var index = 0;
            foreach (var ao in prices.GetAwesome(AwesomeOscillator.DefaultFastPeriods, AwesomeOscillator.DefaultSlowPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.AwesomeOscillator.Oscillator = (decimal?)ao.Oscillator;
                price.Momentum.AwesomeOscillator.Normalized = (decimal?)ao.Normalized;
            }

            index = 0;
            foreach (var bop in prices.GetBop(PriceMomentums.DefaultBopSmoothPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.BalanceOfPower = (decimal?)bop.Bop;
            }

            // Chande Momentum Oscillator
            index = 0;
            foreach (var cmo in prices.GetCmo(PriceMomentums.DefaultCmoLookbackPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.ChandeMomentumOscillator = (decimal?)cmo.Cmo;
            }

            // Commodity Channel Index
            index = 0;
            foreach (var cci in prices.GetCci(PriceMomentums.DefaultCciLookbackPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.CommodityChannelIndex = (decimal?)cci.Cci;
            }

            // Connors RSI
            index = 0;
            foreach (var c in prices.GetConnorsRsi(ConnorsRSI.DefaultRsiPeriods, ConnorsRSI.DefaultStreakPeriods, ConnorsRSI.DefaultRankPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.ConnorsRSI.Rsi = (decimal?)c.Rsi;
                price.Momentum.ConnorsRSI.RsiStreak = (decimal?)c.RsiStreak;
                price.Momentum.ConnorsRSI.PercentRank = (decimal?)c.PercentRank;
                price.Momentum.ConnorsRSI.ConnorsRsi = (decimal?)c.ConnorsRsi;
            }

            // Detrended Price Oscillator
            index = 0;
            foreach (var dpo in prices.GetDpo(DetrendedPriceOscillator.DefaultLookbackPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.DetrendedPriceOscillator.Sma = (decimal?)dpo.Sma;
                price.Momentum.DetrendedPriceOscillator.Dpo = (decimal?)dpo.Dpo;
            }

            // Elder Ray Index
            index = 0;
            foreach (var eri in prices.GetElderRay(ElderRayIndex.DefaultLookbackPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.ElderRayIndex.Bull = (decimal?)eri.BullPower;
                price.Momentum.ElderRayIndex.Bear = (decimal?)eri.BearPower;
            }

            // Gator Oscillator
            index = 0;
            foreach (var g in prices.GetGator())
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.GatorOscillator.Upper = (decimal?)g.Upper;
                price.Momentum.GatorOscillator.Lower = (decimal?)g.Lower;
                price.Momentum.GatorOscillator.UpperExpanding = g.UpperIsExpanding ?? false;
                price.Momentum.GatorOscillator.LowerExpanding = g.LowerIsExpanding ?? false;
            }

            // Moving Average Convergence Divergence
            index = 0;
            foreach (var macd in prices.GetMacd(MovingAverageConvergenceDivergence.DefaultFastPeriods, MovingAverageConvergenceDivergence.DefaultSlowPeriods, MovingAverageConvergenceDivergence.DefaultSignalPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.MACD.Macd = (decimal?)macd.Macd;
                price.Momentum.MACD.Signal = (decimal?)macd.Signal;
                price.Momentum.MACD.Histogram = (decimal?)macd.Histogram;
            }

            // On Balance Volume
            index = 0;
            foreach (var obv in prices.GetObv(OnBalanceVolume.DefaultSmaPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.OnBalanceVolume.Obv = (decimal?)obv.Obv;
                price.Momentum.OnBalanceVolume.Sma = (decimal?)obv.ObvSma;
            }

            // Price Momentum Oscillator
            index = 0;
            foreach (var pmo in prices.GetPmo(PriceMomentumOscillator.DefaultTimePeriods, PriceMomentumOscillator.DefaultSmoothPeriods, PriceMomentumOscillator.DefaultSignalPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.PriceMomentumOscillator.Pmo = (decimal?)pmo.Pmo;
                price.Momentum.PriceMomentumOscillator.Signal = (decimal?)pmo.Signal;
            }

            // Rate of Change
            index = 0;
            foreach (var roc in prices.GetRoc(RateOfChange.DefaultLookbackPeriods, RateOfChange.DefaultSmaPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.RateOfChange.Momentum = (decimal?)roc.Momentum;
                price.Momentum.RateOfChange.Roc = (decimal?)roc.Roc;
                price.Momentum.RateOfChange.Sma = (decimal?)roc.RocSma;
            }

            // Relative Strength Index
            index = 0;
            foreach (var rsi in prices.GetRsi(PriceMomentums.DefaultRsiLookbackPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.RelativeStrengthIndex = (decimal?)rsi.Rsi;
            }

            // ROC with Bands
            index = 0;
            foreach (var rocwb in prices.GetRocWb(RocWithBands.DefaultLookbackPeriods, RocWithBands.DefaultEmaPeriods, RocWithBands.DefaultStdDevPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.RocWithBands.Roc = (decimal?)rocwb.Roc;
                price.Momentum.RocWithBands.Ema = (decimal?)rocwb.RocEma;
                price.Momentum.RocWithBands.Upper = (decimal?)rocwb.UpperBand;
                price.Momentum.RocWithBands.Lower = (decimal?)rocwb.LowerBand;
            }

            // Schaff Trend Cycle
            index = 0;
            foreach (var s in prices.GetStc(PriceMomentums.DefaultStcCyclePeriods, PriceMomentums.DefaultStcFastPeriods, PriceMomentums.DefaultStcSlowPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.SchaffTrendCycle = (decimal?)s.Stc;
            }

            // Stochastic Momentum Index
            index = 0;
            foreach (var smi in prices.GetSmi(StochasticMomentumIndex.DefaultLookbackPeriods, StochasticMomentumIndex.DefaultFirstSmoothPeriods, StochasticMomentumIndex.DefaultSecondSmoothPeriods, StochasticMomentumIndex.DefaultSignalPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.StochasticMomentumIndex.Smi = (decimal?)smi.Smi;
                price.Momentum.StochasticMomentumIndex.Signal = (decimal?)smi.Signal;
            }

            // Stochastic Oscillator
            index = 0;
            foreach (var stoch in prices.GetStoch(StochasticOscillator.DefaultLookbackPeriods, StochasticOscillator.DefaultSignalPeriods, StochasticOscillator.DefaultSmoothPeriods, StochasticOscillator.DefaultKFactor, StochasticOscillator.DefaultDFactor, StochasticOscillator.DefaultMovingAverageType))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.StochasticOscillator.Oscillator = (decimal?)stoch.Oscillator;
                price.Momentum.StochasticOscillator.Signal = (decimal?)stoch.Signal;
                price.Momentum.StochasticOscillator.PercentJ = (decimal?)stoch.PercentJ;
            }
            
            // Stochastic RSI
            index = 0;
            foreach (var srsi in prices.GetStochRsi(StochasticRSI.DefaultRsiPeriods, StochasticRSI.DefaultStochPeriods, StochasticRSI.DefaultSignalPeriods, StochasticRSI.DefaultSmoothPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.StochasticRSI.StochRsi = (decimal?)srsi.StochRsi;
                price.Momentum.StochasticRSI.Signal = (decimal?)srsi.Signal;
            }

            // TRIX
            index = 0;
            foreach (var trix in prices.GetTrix(TripleEmaOscillator.DefaultLookbackPeriods, TripleEmaOscillator.DefaultSignalPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.TRIX.EMA3 = (decimal?)trix.Ema3;
                price.Momentum.TRIX.Trix = (decimal?)trix.Trix;
                price.Momentum.TRIX.Signal = (decimal?)trix.Signal;
            }

            // True Strength Index
            index = 0;
            foreach (var tsi in prices.GetTsi(TrueStrengthIndex.DefaultLookbackPeriods, TrueStrengthIndex.DefaultSmoothPeriods, TrueStrengthIndex.DefaultSignalPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.TrueStrengthIndex.Tsi = (decimal?)tsi.Tsi;
                price.Momentum.TrueStrengthIndex.Signal = (decimal?)tsi.Signal;
            }

            // Ultimate Oscillator
            index = 0;
            foreach (var u in prices.GetUltimate(PriceMomentums.DefaultUltimateShortPeriods, PriceMomentums.DefaultUltimateMiddlePeriods, PriceMomentums.DefaultUltimateLongPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.UltimateOscillator = (decimal?)u.Ultimate;
            }

            // Williams %R
            index = 0;
            foreach (var w in prices.GetWilliamsR(PriceMomentums.DefaultWilliamsLookbackPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.WilliamsR = (decimal?)w.WilliamsR;
            }


            var open = prices.Select(x => (double)x.Open).ToArray();
            var high = prices.Select(x => (double)x.High).ToArray();
            var low = prices.Select(x => (double)x.Low).ToArray();
            var close = prices.Select(x => (double)x.Close).ToArray();
            var volume = prices.Select(x => (double)x.Volume).ToArray();
            var time = prices.Select(x => x.TimeOpen.DateTime).ToArray();

            // Absolute Price Oscillator
            var results = new StockData(open, high, low, close, volume, time).CalculateAbsolutePriceOscillator();
            var apo = results.OutputValues["Apo"];

            // Commodity Selection Index
            results = new StockData(open, high, low, close, volume, time).CalculateCommoditySelectionIndex();
            var csi = results.OutputValues["Csi"];
            var csiSignal = results.OutputValues["Signal"];

            // Percentage Price Oscillator
            results = new StockData(open, high, low, close, volume, time).CalculatePercentagePriceOscillator();
            var ppo = results.OutputValues["Ppo"];
            var signal = results.OutputValues["Signal"];
            var histogram = results.OutputValues["Histogram"];

            // Stochastic Fast Oscillator
            results = new StockData(open, high, low, close, volume, time).CalculateStochasticFastOscillator();
            var sfo = results.OutputValues["Sfo"];
            var sfoSignal = results.OutputValues["Signal"];

            for (var i = 0; i < prices.Length; i++)
            {
                var price = prices[i];
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Momentum.AbsolutePriceOscillator = (decimal?)apo[i];

                price.Momentum.CommoditySelectionIndex.Csi = (decimal?)csi[i];
                price.Momentum.CommoditySelectionIndex.Signal = (decimal?)csiSignal[i];

                price.Momentum.PercentagePriceOscillator.Ppo = (decimal?)ppo[i];
                price.Momentum.PercentagePriceOscillator.Signal = (decimal?)signal[i];
                price.Momentum.PercentagePriceOscillator.Histogram = (decimal?)histogram[i];

                price.Momentum.StochasticFastOscillator.Sfo = (decimal?)sfo[i];
                price.Momentum.StochasticFastOscillator.Signal = (decimal?)sfoSignal[i];
            }
        }
        public static void UpdateMovingAverages(Price[] prices, DateTimeOffset start)
        {
            foreach (var price in prices)
            {
                if (price.MA == null)
                {
                    price.MA = new PriceMovingAverages();
                }
            }

            var index = 0;
            foreach (var alma in prices.GetAlma(6, PriceMovingAverages.DefaultAlmaOffset, PriceMovingAverages.DefaultAlmaSigma))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.ALMA6 = (decimal?)alma.Alma;
            }

            index = 0;
            foreach (var alma in prices.GetAlma(12, PriceMovingAverages.DefaultAlmaOffset, PriceMovingAverages.DefaultAlmaSigma))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.ALMA12 = (decimal?)alma.Alma;
            }

            index = 0;
            foreach (var alma in prices.GetAlma(24, PriceMovingAverages.DefaultAlmaOffset, PriceMovingAverages.DefaultAlmaSigma))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.ALMA24 = (decimal?)alma.Alma;
            }

            index = 0;
            foreach (var alma in prices.GetAlma(168, PriceMovingAverages.DefaultAlmaOffset, PriceMovingAverages.DefaultAlmaSigma))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.ALMA168 = (decimal?)alma.Alma;
            }

            index = 0;
            foreach (var dema in prices.GetDema(6))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.DEMA6 = (decimal?)dema.Dema;
            }

            index = 0;
            foreach (var dema in prices.GetDema(12))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.DEMA12 = (decimal?)dema.Dema;
            }

            index = 0;
            foreach (var dema in prices.GetDema(24))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.DEMA24 = (decimal?)dema.Dema;
            }

            index = 0;
            foreach (var dema in prices.GetDema(168))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.DEMA168 = (decimal?)dema.Dema;
            }

            index = 0;
            foreach (var ema in prices.GetEma(6))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.EMA6 = (decimal?)ema.Ema;
            }

            index = 0;
            foreach (var ema in prices.GetEma(12))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.EMA12 = (decimal?)ema.Ema;
            }

            index = 0;
            foreach (var ema in prices.GetEma(24))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.EMA24 = (decimal?)ema.Ema;
            }

            index = 0;
            foreach (var ema in prices.GetEma(168))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.EMA168 = (decimal?)ema.Ema;
            }

            index = 0;
            foreach (var epma in prices.GetEpma(6))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.EPMA6 = (decimal?)epma.Epma;
            }

            index = 0;
            foreach (var epma in prices.GetEpma(12))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.EPMA12 = (decimal?)epma.Epma;
            }

            index = 0;
            foreach (var epma in prices.GetEpma(24))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.EPMA24 = (decimal?)epma.Epma;
            }

            index = 0;
            foreach (var epma in prices.GetEpma(168))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.EPMA168 = (decimal?)epma.Epma;
            }

            index = 0;
            foreach (var hma in prices.GetHma(6))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.HMA6 = (decimal?)hma.Hma;
            }

            index = 0;
            foreach (var hma in prices.GetHma(12))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.HMA12 = (decimal?)hma.Hma;
            }

            index = 0;
            foreach (var hma in prices.GetHma(24))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.HMA24 = (decimal?)hma.Hma;
            }

            index = 0;
            foreach (var hma in prices.GetHma(168))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.HMA168 = (decimal?)hma.Hma;
            }

            index = 0;
            foreach (var kama in prices.GetKama(KaufmansAdaptiveMovingAverage.DefaultErPeriods, KaufmansAdaptiveMovingAverage.DefaultFastPeriods, KaufmansAdaptiveMovingAverage.DefaultSlowPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.KAMA.ER = (decimal?)kama.ER;
                price.MA.KAMA.Kama = (decimal?)kama.Kama;
            }

            index = 0;
            foreach (var mama in prices.GetMama(MesaAdaptiveMovingAverage.DefaultFastLimit, MesaAdaptiveMovingAverage.DefaultSlowLimit))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.MAMA.MAMA = (decimal?)mama.Mama;
                price.MA.MAMA.FAMA = (decimal?)mama.Fama;
            }

            index = 0;
            foreach (var sma in prices.GetSma(12).GetSmaAnalysis(12))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.SMA12.Sma = (decimal?)sma.Sma;
                price.MA.SMA12.Mad = (decimal?)sma.Mad;
                price.MA.SMA12.Mse = (decimal?)sma.Mse;
                price.MA.SMA12.Mape = (decimal?)sma.Mape;
            }

            index = 0;
            foreach (var sma in prices.GetSma(24).GetSmaAnalysis(24))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.SMA24.Sma = (decimal?)sma.Sma;
                price.MA.SMA24.Mad = (decimal?)sma.Mad;
                price.MA.SMA24.Mse = (decimal?)sma.Mse;
                price.MA.SMA24.Mape = (decimal?)sma.Mape;
            }

            index = 0;
            foreach (var sma in prices.GetSma(168).GetSmaAnalysis(168))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.SMA168.Sma = (decimal?)sma.Sma;
                price.MA.SMA168.Mad = (decimal?)sma.Mad;
                price.MA.SMA168.Mse = (decimal?)sma.Mse;
                price.MA.SMA168.Mape = (decimal?)sma.Mape;
            }

            index = 0;
            foreach (var smma in prices.GetSmma(6))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.SMMA6 = (decimal?)smma.Smma;
            }

            index = 0;
            foreach (var smma in prices.GetSmma(12))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.SMMA12 = (decimal?)smma.Smma;
            }

            index = 0;
            foreach (var smma in prices.GetSmma(24))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.SMMA24 = (decimal?)smma.Smma;
            }

            index = 0;
            foreach (var smma in prices.GetSmma(168))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.SMMA168 = (decimal?)smma.Smma;
            }

            index = 0;
            foreach (var t3 in prices.GetT3(6))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.T3_6 = (decimal?)t3.T3;
            }

            index = 0;
            foreach (var t3 in prices.GetT3(12))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.T3_12 = (decimal?)t3.T3;
            }

            index = 0;
            foreach (var t3 in prices.GetT3(24))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.T3_24 = (decimal?)t3.T3;
            }

            index = 0;
            foreach (var t3 in prices.GetT3(168))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.T3_168 = (decimal?)t3.T3;
            }

            index = 0;
            foreach (var tema in prices.GetTema(6))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.TEMA6 = (decimal?)tema.Tema;
            }

            index = 0;
            foreach (var tema in prices.GetTema(12))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.TEMA12 = (decimal?)tema.Tema;
            }

            index = 0;
            foreach (var tema in prices.GetTema(24))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.TEMA24 = (decimal?)tema.Tema;
            }

            index = 0;
            foreach (var tema in prices.GetTema(168))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.TEMA168 = (decimal?)tema.Tema;
            }

            index = 0;
            foreach (var wma in prices.GetWma(6))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.WMA6 = (decimal?)wma.Wma;
            }

            index = 0;
            foreach (var wma in prices.GetWma(12))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.WMA12 = (decimal?)wma.Wma;
            }

            index = 0;
            foreach (var wma in prices.GetWma(24))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.WMA24 = (decimal?)wma.Wma;
            }

            index = 0;
            foreach (var wma in prices.GetWma(168))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.WMA168 = (decimal?)wma.Wma;
            }

            index = 0;
            foreach (var vwma in prices.GetVwma(6))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.VWMA6 = (decimal?)vwma.Vwma;
            }

            index = 0;
            foreach (var vwma in prices.GetVwma(12))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.VWMA12 = (decimal?)vwma.Vwma;
            }

            index = 0;
            foreach (var vwma in prices.GetVwma(24))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.VWMA24 = (decimal?)vwma.Vwma;
            }

            index = 0;
            foreach (var vwma in prices.GetVwma(168))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.MA.VWMA168 = (decimal?)vwma.Vwma;
            }
        }
        public static void UpdateOtherIndicators(Price[] prices, DateTimeOffset start)
        {
            foreach (var price in prices)
            {
                if (price.OtherIndicators == null)
                {
                    price.OtherIndicators = new PriceOtherIndicators();
                }
            }

            // Fractal Chaos Bands
            var index = 0;
            foreach (var d in prices.GetFcb(FractalChaosBand.DefaultWindowSpan))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.OtherIndicators.FractalChaosBands.Upper = d.UpperBand;
                price.OtherIndicators.FractalChaosBands.Lower = d.LowerBand;
            }

            // Hurst Exponent
            index = 0;
            foreach (var he in prices.GetHurst(PriceOtherIndicators.DefaultHurstExponentLoopbackPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.OtherIndicators.HurstExponent = (decimal?)he.HurstExponent;
            }

            // Pivot
            index = 0;
            foreach (var p in prices.GetPivots(Pivot.DefaultLeftSpan, Pivot.DefaultRightSpan, Pivot.DefaultMaxTrendPeriods, Pivot.DefaultEndType))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.OtherIndicators.Pivot.HighPoint = (decimal?)p.HighPoint;
                price.OtherIndicators.Pivot.LowPoint = (decimal?)p.LowPoint;
                price.OtherIndicators.Pivot.HighLine = (decimal?)p.HighLine;
                price.OtherIndicators.Pivot.LowLine = (decimal?)p.LowLine;
                if(p.HighTrend != null)
                {
                    price.OtherIndicators.Pivot.HighTrend = (Misc.PivotTrend)((int)(p.HighTrend) + 1);
                }
                if(p.LowTrend != null)
                {
                    price.OtherIndicators.Pivot.LowTrend = (Misc.PivotTrend)((int)(p.LowTrend) + 1);      
                }
            }

            // Pivot Points
            index = 0;
            foreach (var p in prices.GetPivotPoints(PivotPoints.DefaultWindowSize, PivotPoints.DefaultPivotPointType))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.OtherIndicators.PivotPoints.R1 = p.R1;
                price.OtherIndicators.PivotPoints.R2 = p.R2;
                price.OtherIndicators.PivotPoints.R3 = p.R3;
                price.OtherIndicators.PivotPoints.PP = p.PP;
                price.OtherIndicators.PivotPoints.S1 = p.S1;
                price.OtherIndicators.PivotPoints.S2 = p.S2;
                price.OtherIndicators.PivotPoints.S3 = p.S3;
            }

            // Price Relative Strength
            /*
            index = 0;
            foreach (var p in prices.GetPrs(PriceRelativeStrength.DefaultLoopbackPeriods, PriceRelativeStrength.DefaultSmaPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.OtherIndicators.PivotPoints.R1 = p.R1;
                price.OtherIndicators.PivotPoints.R2 = p.R2;
                price.OtherIndicators.PivotPoints.R3 = p.R3;
                price.OtherIndicators.PivotPoints.PP = p.PP;
                price.OtherIndicators.PivotPoints.S1 = p.S1;
                price.OtherIndicators.PivotPoints.S2 = p.S2;
                price.OtherIndicators.PivotPoints.S3 = p.S3;
            }
            */

            // Rolling Pivot Points
            index = 0;
            foreach (var p in prices.GetRollingPivots(PivotPoints.DefaultRollingWindowPeriods, PivotPoints.DefaultRollingOffsetPeriods, PivotPoints.DefaultRollingPivotPointType))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.OtherIndicators.RollingPivotPoints.R1 = p.R1;
                price.OtherIndicators.RollingPivotPoints.R2 = p.R2;
                price.OtherIndicators.RollingPivotPoints.R3 = p.R3;
                price.OtherIndicators.RollingPivotPoints.PP = p.PP;
                price.OtherIndicators.RollingPivotPoints.S1 = p.S1;
                price.OtherIndicators.RollingPivotPoints.S2 = p.S2;
                price.OtherIndicators.RollingPivotPoints.S3 = p.S3;
            }

            // Williams Fractal
            index = 0;
            foreach (var wf in prices.GetFractal(WilliamsFractal.DefaultWindowSpan, WilliamsFractal.DefaultEndType))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.OtherIndicators.WilliamsFractal.FractalBear = (decimal?)wf.FractalBear;
                price.OtherIndicators.WilliamsFractal.FractalBull = (decimal?)wf.FractalBull;
            }

            var open = prices.Select(x => (double)x.Open).ToArray();
            var high = prices.Select(x => (double)x.High).ToArray();
            var low = prices.Select(x => (double)x.Low).ToArray();
            var close = prices.Select(x => (double)x.Close).ToArray();
            var volume = prices.Select(x => (double)x.Volume).ToArray();
            var time = prices.Select(x => x.TimeOpen.DateTime).ToArray();

            // Vertical Horizontal Filter
            var results = new StockData(open, high, low, close, volume, time).CalculateVerticalHorizontalFilter();
            var vhf = results.OutputValues["Vhf"];
            var signal = results.OutputValues["Signal"];

            // Projection Oscillator
            results = new StockData(open, high, low, close, volume, time).CalculateProjectionOscillator();
            var pbo = results.OutputValues["Pbo"];
            var pboSignal = results.OutputValues["Signal"];

            for (var i = 0; i < prices.Length; i++)
            {
                var price = prices[i];
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.OtherIndicators.VerticalHorizontalFilter.Vhf = (decimal?)vhf[i];
                price.OtherIndicators.VerticalHorizontalFilter.Signal = (decimal?)signal[i];

                price.OtherIndicators.ProjectionOscillator.Pbo = (decimal?)pbo[i];
                price.OtherIndicators.ProjectionOscillator.Signal = (decimal?)pboSignal[i];
            }
        }
        public static void UpdatePeaks(Price[] prices, DateTimeOffset start, DateTimeOffset end)
        {
            var previousHHIndex = 0;
            var previousLLIndex = 0;
            if (prices.Any())
            {
                prices[0].Peak = prices[0].Peak ?? new PricePeak();
            }
            for (var i = 1; i < prices.Length - 1; i++)
            {
                var current = prices[i];
                if (current.TimeOpen < start || current.TimeOpen > end)
                {
                    continue;
                }

                current.Peak = current.Peak ?? new PricePeak();

                var prevIndex = i - 1;
                var previous = prices[prevIndex];

                while (previous.Low == current.Low && prevIndex > 0)
                {
                    prevIndex--;
                    previous = prices[prevIndex];
                }

                var nextIndex = i + 1;
                var next = prices[nextIndex];
                while (next.Low == current.Low && nextIndex < prices.Length - 1)
                {
                    nextIndex++;
                    next = prices[nextIndex];
                }

                current.Peak.OffsetPreviousLL = i - previousLLIndex;
                if (previous.Low > current.Low && current.Low < next.Low)
                {
                    current.Peak.LowestLow = true;
                    for(var j = previousLLIndex; j < i; j++)
                    {
                        prices[j].Peak = prices[j].Peak ?? new PricePeak();
                        prices[j].Peak.OffsetNextLL = i - j;
                    }
                    previousLLIndex = i;
                }


                prevIndex = i - 1;
                nextIndex = i + 1;
                while (previous.High == current.High && prevIndex > 0)
                {
                    prevIndex--;
                    previous = prices[prevIndex];
                }
                while (next.High == current.High && nextIndex < prices.Length - 1)
                {
                    nextIndex++;
                    next = prices[nextIndex];
                }

                current.Peak.OffsetPreviousHH = i - previousHHIndex;
                if (previous.High < current.High && current.High > next.High)
                {
                    current.Peak.HighestHigh = true;
                    for(var j = previousHHIndex; j < i; j++)
                    {
                        prices[j].Peak = prices[j].Peak ?? new PricePeak();
                        prices[j].Peak.OffsetNextHH = i - j;
                    }

                    previousHHIndex = i;
                }
            }
        }
        public static void UpdateReturns(Price[] prices)
        {
            var hoursInDay = 24;
            var hoursInTwoDays = 2 * 24;
            var hoursInWeek = 7 * 24;

            (int, Price) FindNextMaxHigh(int currentIndex, int hours)
            {
                var highestIndex = -1;
                for(var h = 1; h <= hours; h++)
                {
                    var index = currentIndex + h;
                    if (index >= prices.Length)
                    {
                        break;
                    }
                    if (prices[index].TimeOpen > prices[currentIndex].TimeOpen.AddHours(hours))
                    {
                        break;
                    }
                    
                    if (highestIndex < 0 || prices[index].High > prices[highestIndex].High)
                    {
                        highestIndex = index;
                    }
                }

                if(highestIndex >= 0)
                {
                    return (highestIndex, prices[highestIndex]);
                }

                return (-1, null);
            }

            for (var i = 0; i < prices.Length; i++)
            {
                var current = prices[i];
                current.Return = current.Return ?? new PriceReturn
                {
                };

                var time = current.TimeOpen.AddHours(hoursInDay);
                if (time > prices.Last().TimeOpen)
                {
                    break;
                }

                var purchasePrice = i == 0 ? current.Low : (current.Low + prices[i - 1].Low) / 2;

                var (index24h, max24h) = FindNextMaxHigh(i, hoursInDay);
                if (max24h != null)
                {
                    var previous = prices[index24h - 1];
                    var sellPrice = (max24h.High + previous.High) / 2;
                    current.Return.Day.Return = (sellPrice - purchasePrice) / purchasePrice;
                    current.Return.Day.Interval = (int)(max24h.TimeOpen - current.TimeOpen).TotalHours;
                }

                time = current.TimeOpen.AddHours(hoursInTwoDays);
                if (time > prices.Last().TimeOpen)
                {
                    continue;
                }
                var (index48h, max48h) = FindNextMaxHigh(i, hoursInTwoDays);
                if (max48h != null)
                {
                    var previous = prices[index48h - 1];
                    var sellPrice = (max48h.High + previous.High) / 2;
                    current.Return.TwoDay.Return = (sellPrice - purchasePrice) / purchasePrice;
                    current.Return.TwoDay.Interval = (int)(max48h.TimeOpen - current.TimeOpen).TotalHours;
                }

                time = current.TimeOpen.AddHours(hoursInWeek);
                if (time > prices.Last().TimeOpen)
                {
                    continue;
                }
                var (indexWeek, maxWeek) = FindNextMaxHigh(i, hoursInWeek);
                if (maxWeek != null)
                {
                    var previous = prices[indexWeek - 1];
                    var sellPrice = (maxWeek.High + previous.High) / 2;
                    current.Return.Week.Return = (sellPrice - purchasePrice) / purchasePrice;
                    current.Return.Week.Interval = (int)(maxWeek.TimeOpen - current.TimeOpen).TotalHours;
                }

            }
            for (var i = 0; i < prices.Length; i++)
            {
                var current = prices[i];
                if(i + 11 > prices.Length || prices[i + 11].Return == null)
                {
                    break;
                }
                var startIndex = Math.Max(i - 12, 0);
                var prices24 = prices[startIndex..(startIndex + 24)].OrderByDescending(x => x.Return.Day.Return).ToArray();
                var index24 = Array.IndexOf(prices24, current);
                current.Return.Day.Rank2 = index24 / 12 + 1;
                current.Return.Day.Rank3 = index24 / 8 + 1;
                current.Return.Day.Rank4 = index24 / 6 + 1;
                current.Return.Day.Rank6 = index24 / 4 + 1;
                current.Return.Day.Rank8 = index24 / 3 + 1;
                current.Return.Day.Rank12 = index24 / 2 + 1;

                if (i + 23 > prices.Length || prices[i + 23].Return == null)
                {
                    continue;
                }
                startIndex = Math.Max(i - 24, 0);
                var prices48 = prices[startIndex..(startIndex + 48)].OrderByDescending(x => x.Return.TwoDay.Return).ToArray();
                var index48 = Array.IndexOf(prices48, current);
                current.Return.TwoDay.Rank2 = index48 / 24 + 1;
                current.Return.TwoDay.Rank3 = index48 / 16 + 1;
                current.Return.TwoDay.Rank4 = index48 / 12 + 1;
                current.Return.TwoDay.Rank6 = index48 / 8 + 1;
                current.Return.TwoDay.Rank8 = index48 / 6 + 1;
                current.Return.TwoDay.Rank12 = index48 / 4 + 1;
      
                if (i + 83 >= prices.Length || prices[i + 83].Return == null)
                {
                    continue;
                }
                startIndex = Math.Max(i - 84, 0);
                var pricesWeek = prices[startIndex..(startIndex + 168)].OrderByDescending(x => x.Return.Week.Return).ToArray();
                var indexWeek = Array.IndexOf(pricesWeek, current);
                current.Return.Week.Rank2 = indexWeek / 84 + 1;
                current.Return.Week.Rank3 = indexWeek / 56 + 1;
                current.Return.Week.Rank4 = indexWeek / 42 + 1;
                current.Return.Week.Rank6 = indexWeek / 28 + 1;
                current.Return.Week.Rank8 = indexWeek / 21 + 1;
                current.Return.Week.Rank12 = indexWeek / 14 + 1;
            }                               
        }
        public async Task UpdateSlopes(Price[] prices, DateTimeOffset start, DateTimeOffset end)
        {
            if(end > DateTimeOffset.UtcNow)
            {
                end = DateTimeOffset.UtcNow.StartOfHour().AddMilliseconds(-1);
            }
            var windowStart = start;
            while (windowStart < end)
            {
                var windowEnd = end;
                if ((windowEnd - windowStart).TotalDays > 30)
                {
                    windowEnd = windowStart.AddDays(30);
                }

                var dataStart = windowStart.AddHours(-24);
                var priceBatch = prices.Where(x => x.TimeOpen >= dataStart && x.TimeOpen < windowEnd).OrderBy(x => x.TimeOpen).ToArray();
                var priceBatchLength = priceBatch.Length;
                var expectedLength = (windowEnd - dataStart).TotalHours;
                if(priceBatchLength < expectedLength * 0.9)
                {
                    windowStart = windowEnd;
                    continue;
                }
                
                var savgolResponse = await _predictionService.GetSavgol(new SavgolRequest
                {
                    Data =
                    [
                        new SavgolRequestValues { Field = nameof(Price.High), Values = priceBatch.Select(x => x.High).ToArray() },
                        new SavgolRequestValues { Field = nameof(Price.Low), Values = priceBatch.Select(x => x.Low).ToArray() },
                    ],
                    Order = 3,
                    Windows = [6, 8, 12, 24]
                });

                for (var i = 0; i < priceBatch.Length; i++)
                {
                    if (priceBatch[i].TimeOpen < windowStart)
                    {
                        continue;
                    }
                    priceBatch[i].Trend = priceBatch[i].Trend ?? new PriceTrends();

                    priceBatch[i].Trend.Slope.High6 = savgolResponse.First(x => x.Window == 6 && x.Field == nameof(Price.High)).Derivatives[i];
                    priceBatch[i].Trend.Slope.High8 = savgolResponse.First(x => x.Window == 8 && x.Field == nameof(Price.High)).Derivatives[i];
                    priceBatch[i].Trend.Slope.High12 = savgolResponse.First(x => x.Window == 12 && x.Field == nameof(Price.High)).Derivatives[i];
                    priceBatch[i].Trend.Slope.High24 = savgolResponse.First(x => x.Window == 24 && x.Field == nameof(Price.High)).Derivatives[i];
                    priceBatch[i].Trend.Slope.Low6 = savgolResponse.First(x => x.Window == 6 && x.Field == nameof(Price.Low)).Derivatives[i];
                    priceBatch[i].Trend.Slope.Low8 = savgolResponse.First(x => x.Window == 8 && x.Field == nameof(Price.Low)).Derivatives[i];
                    priceBatch[i].Trend.Slope.Low12 = savgolResponse.First(x => x.Window == 12 && x.Field == nameof(Price.Low)).Derivatives[i];
                    priceBatch[i].Trend.Slope.Low24 = savgolResponse.First(x => x.Window == 24 && x.Field == nameof(Price.Low)).Derivatives[i];
                }

                windowStart = windowEnd;
            }
        }
        public static void UpdateTrends(Price[] prices, DateTimeOffset start)
        {
            foreach (var price in prices)
            {
                if (price.Trend == null)
                {
                    price.Trend = new PriceTrends();
                }
            }

            // Aroon
            var index = 0;
            foreach (var aroon in prices.GetAroon(Aroon.DefaultLookbackPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Trend.Aroon.Up = (decimal?)aroon.AroonUp;
                price.Trend.Aroon.Down = (decimal?)aroon.AroonDown;
                price.Trend.Aroon.Oscillator = (decimal?)aroon.Oscillator;
            }

            // ATR Trailing Stop
            index = 0;
            foreach (var atrStop in prices.GetAtrStop(AtrTrailingStop.DefaultLookbackPeriods, AtrTrailingStop.DefaultMultiplier, AtrTrailingStop.DefaultEndType))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Trend.AtrTrailingStop.AtrStop = atrStop.AtrStop;
                price.Trend.AtrTrailingStop.BuyStop = atrStop.BuyStop;
                price.Trend.AtrTrailingStop.SellStop = atrStop.SellStop;
            }

            // Average Directional Index / Directional Movement Index
            index = 0;
            foreach (var adx in prices.GetAdx(AverageDirectionalIndex.DefaultLookbackPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Trend.ADX.Pdi = (decimal?)adx.Pdi;
                price.Trend.ADX.Mdi = (decimal?)adx.Mdi;
                price.Trend.ADX.Adx = (decimal?)adx.Adx;
                price.Trend.ADX.Adxr = (decimal?)adx.Adxr;
            }

            // Chandelier Exit
            index = 0;
            foreach (var c in prices.GetChandelier(ChandelierExit.DefaultLookbackPeriods, ChandelierExit.DefaultMultiplier, ChandelierType.Short))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Trend.ChandelierExit.Short = (decimal?)c.ChandelierExit;
            }
            index = 0;
            foreach (var c in prices.GetChandelier(ChandelierExit.DefaultLookbackPeriods, ChandelierExit.DefaultMultiplier, ChandelierType.Long))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Trend.ChandelierExit.Long = (decimal?)c.ChandelierExit;
            }


            // Hilbert Transform Instantaneous Trendline
            index = 0;
            foreach (var ht in prices.GetHtTrendline())
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Trend.HilbertTransform.DominantCyclePeriods = ht.DcPeriods;
                price.Trend.HilbertTransform.TrendLine = (decimal?)ht.Trendline;
                price.Trend.HilbertTransform.SmoothPrice = (decimal?)ht.SmoothPrice;
            }

            // Ichimoku Cloud
            index = 0;
            foreach (var i in prices.GetIchimoku(IchimokuCloud.DefaultTenkanPeriods, IchimokuCloud.DefaultKijunPeriods, IchimokuCloud.DefaultSenkouSpanPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Trend.IchimokuCloud.Signal = i.TenkanSen;
                price.Trend.IchimokuCloud.Base = i.KijunSen;
                price.Trend.IchimokuCloud.LeadingA = i.SenkouSpanA;
                price.Trend.IchimokuCloud.LeadingB = i.SenkouSpanB;
                price.Trend.IchimokuCloud.Lagging = i.ChikouSpan;
            }

            // McGinley Dynamic
            var mgd6 = prices.GetDynamic(6).ToArray();
            var mgd12 = prices.GetDynamic(12).ToArray();
            var mgd24 = prices.GetDynamic(24).ToArray();
            var mgd168 = prices.GetDynamic(168).ToArray();
            for (var i = 0; i < prices.Length; i++)
            {
                var price = prices[i];
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Trend.McGinleyDynamic.McGinleyDynamic6 = (decimal?)mgd6[i].Dynamic;
                price.Trend.McGinleyDynamic.McGinleyDynamic12 = (decimal?)mgd12[i].Dynamic;
                price.Trend.McGinleyDynamic.McGinleyDynamic24 = (decimal?)mgd24[i].Dynamic;
                price.Trend.McGinleyDynamic.McGinleyDynamic168 = (decimal?)mgd168[i].Dynamic;
            }
            

            // Moving Average Envelope
            index = 0;
            foreach (var e in prices.GetMaEnvelopes(MovingAverageEnvelope.DefaultLoopbackPeriods, MovingAverageEnvelope.DefaultPercentOffset, MovingAverageEnvelope.DefaultType))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Trend.MovingAverageEnvelope.Upper = (decimal?)e.UpperEnvelope;
                price.Trend.MovingAverageEnvelope.Center = (decimal?)e.Centerline;
                price.Trend.MovingAverageEnvelope.Lower = (decimal?)e.LowerEnvelope;
            }

            // Parabolic SAR
            index = 0;
            foreach (var p in prices.GetParabolicSar(ParabolicSAR.DefaultAccelerationFactor, ParabolicSAR.DefaultMaxAccelerationFactor))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Trend.ParabolicSAR.SAR = (decimal?)p.Sar;
                price.Trend.ParabolicSAR.IsReversal = p.IsReversal ?? false;
            }

            // SuperTrend
            index = 0;
            foreach (var super in prices.GetSuperTrend(SuperTrend.DefaultLookbackPeriods, SuperTrend.DefaultMultiplier))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Trend.SuperTrend.Combined = super.SuperTrend;
                price.Trend.SuperTrend.Upper = super.UpperBand;
                price.Trend.SuperTrend.Lower = super.LowerBand;
            }

            // Vortex Indicator
            index = 0;
            foreach (var v in prices.GetVortex(VortexIndicator.DefaultLookbackPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Trend.VortexIndicator.Pvi = (decimal?)v.Pvi;
                price.Trend.VortexIndicator.Nvi = (decimal?)v.Nvi;
            }

            // Williams Alligator
            index = 0;
            foreach (var w in prices.GetAlligator(WilliamsAlligator.DefaultJawPeriods, WilliamsAlligator.DefaultJawOffset, WilliamsAlligator.DefaultTeethPeriods, WilliamsAlligator.DefaultTeethOffset, WilliamsAlligator.DefaultLipsPeriods, WilliamsAlligator.DefaultLipsOffset))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Trend.WilliamsAlligator.Jaw = (decimal?)w.Jaw;
                price.Trend.WilliamsAlligator.Teeth = (decimal?)w.Teeth;
                price.Trend.WilliamsAlligator.Lips = (decimal?)w.Lips;
            }

            /*
            var open = prices.Select(x => (double)x.Open).ToArray();
            var high = prices.Select(x => (double)x.High).ToArray();
            var low = prices.Select(x => (double)x.Low).ToArray();
            var close = prices.Select(x => (double)x.Close).ToArray();
            var volume = prices.Select(x => (double)x.Volume).ToArray();
            var time = prices.Select(x => x.TimeOpen.DateTime).ToArray();

            var results = new StockData(open, high, low, close, volume, time).CalculateLinearRegression();
            var lr = results.OutputValues["Apo"];

            for (var i = 0; i < prices.Length; i++)
            {
                var price = prices[i];
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Trends.LinearRegression = (decimal?)apo[i];
            }
            */
        }
        public static void UpdateVolatilities(Price[] prices, DateTimeOffset start)
        {
            foreach (var price in prices)
            {
                if (price.Volatility == null)
                {
                    price.Volatility = new PriceVolatilities();
                }
            }

            // Average True Range
            var atr12 = prices.GetAtr(12).ToArray();
            var atr24 = prices.GetAtr(24).ToArray();
            var atr168 = prices.GetAtr(168).ToArray();
            for (var i = 0; i < prices.Length; i++)
            {
                var price = prices[i];
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Volatility.ATR12.TR = (decimal?)atr12[i].Tr;
                price.Volatility.ATR12.Atr = (decimal?)atr12[i].Atr;
                price.Volatility.ATR12.Atrp = (decimal?)atr12[i].Atrp;
                price.Volatility.ATR24.TR = (decimal?)atr24[i].Tr;
                price.Volatility.ATR24.Atr = (decimal?)atr24[i].Atr;
                price.Volatility.ATR24.Atrp = (decimal?)atr24[i].Atrp;
                price.Volatility.ATR168.TR = (decimal?)atr168[i].Tr;
                price.Volatility.ATR168.Atr = (decimal?)atr168[i].Atr;
                price.Volatility.ATR168.Atrp = (decimal?)atr168[i].Atrp;
            }   

            // Bollinger Bands
            var index = 0;
            foreach (var bb in prices.GetBollingerBands(BollingerBand.DefaultLoopbackPeriods, BollingerBand.DefaultStandardDeviation))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Volatility.BollingerBands.Upper = (decimal?)bb.UpperBand;
                price.Volatility.BollingerBands.Lower = (decimal?)bb.LowerBand;
                price.Volatility.BollingerBands.PercentB = (decimal?)bb.PercentB;
                price.Volatility.BollingerBands.ZScore = (decimal?)bb.ZScore;
                price.Volatility.BollingerBands.Width = (decimal?)bb.Width;
            }

            // Choppiness Index
            index = 0;
            foreach (var c in prices.GetChop(PriceVolatilities.DefaultChopLoopbackPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Volatility.ChoppinessIndex = (decimal?)c.Chop;
            }

            // Donchian Channel
            index = 0;
            foreach (var d in prices.GetDonchian(DonchianChannel.DefaultLoopbackPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Volatility.DonchianChannels.Upper = d.UpperBand;
                price.Volatility.DonchianChannels.Center = d.Centerline;
                price.Volatility.DonchianChannels.Lower = d.LowerBand;
                price.Volatility.DonchianChannels.Width = d.Width;
            }

            // Kelner Channel
            index = 0;
            foreach (var k in prices.GetKeltner(KeltnerChannel.DefaultEmaPeriods, KeltnerChannel.DefaultMultiplier, KeltnerChannel.DefaultAtrPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Volatility.KeltnerChannels.Upper = (decimal?)k.UpperBand;
                price.Volatility.KeltnerChannels.Center = (decimal?)k.Centerline;
                price.Volatility.KeltnerChannels.Lower = (decimal?)k.LowerBand;
                price.Volatility.KeltnerChannels.Width = (decimal?)k.Width;
            }

            // Standard Deviation Channel
            index = 0;
            foreach (var sdc in prices.GetStdDevChannels(StandardDeviationChannel.DefaultLookbackPeriods, StandardDeviationChannel.DefaultStandardDeviations))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Volatility.StandardDeviationChannel.Center = (decimal?)sdc.Centerline;
                price.Volatility.StandardDeviationChannel.Upper = (decimal?)sdc.UpperChannel;
                price.Volatility.StandardDeviationChannel.Lower = (decimal?)sdc.LowerChannel;
                price.Volatility.StandardDeviationChannel.BreakPoint = sdc.BreakPoint;
            }

            // Starc Bands
            index = 0;
            foreach (var s in prices.GetStarcBands(StarcBand.DefaultSmaPeriods, StarcBand.DefaultMultiplier, StarcBand.DefaultAtrPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Volatility.StarcBand.Upper = (decimal?)s.UpperBand;
                price.Volatility.StarcBand.Center = (decimal?)s.Centerline;
                price.Volatility.StarcBand.Lower = (decimal?)s.LowerBand;
            }

            // Ulcer Index
            index = 0;
            foreach (var u in prices.GetUlcerIndex(PriceVolatilities.DefaultUlcerLoopbackPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Volatility.UlcerIndex = (decimal?)u.UI;
            }

            // Volatility Stop
            index = 0;
            foreach (var v in prices.GetVolatilityStop(VolatilityStop.DefaultLookbackPeriods, VolatilityStop.DefaultMultiplier))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Volatility.VolatilityStop.SAR = (decimal?)v.Sar;
                price.Volatility.VolatilityStop.IsStop = v.IsStop ?? false;
                price.Volatility.VolatilityStop.Upper = (decimal?)v.UpperBand;
                price.Volatility.VolatilityStop.Lower = (decimal?)v.LowerBand;
            }

            var open = prices.Select(x => (double)x.Open).ToArray();
            var high = prices.Select(x => (double)x.High).ToArray();
            var low = prices.Select(x => (double)x.Low).ToArray();
            var close = prices.Select(x => (double)x.Close).ToArray();
            var volume = prices.Select(x => (double)x.Volume).ToArray();
            var time = prices.Select(x => x.TimeOpen.DateTime).ToArray();

            // Historical Volatility
            var results = new StockData(open, high, low, close, volume, time).CalculateHistoricalVolatility();
            var hv = results.OutputValues["Hv"];

            for (var i = 0; i < prices.Length; i++)
            {
                var price = prices[i];
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Volatility.HistoricalVolatility = (decimal)hv[i];
            }
        }
        public static void UpdateVolumes(Price[] prices, DateTimeOffset start)
        {
            foreach (var price in prices)
            {
                if (price.Volumes == null)
                {
                    price.Volumes = new PriceVolumes();
                }
            }

            var index = 0;
            foreach (var adl in prices.GetAdl(AccumulationDistributionLine.DefaultSmaPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Volumes.AccumulationDistributionLine.MoneyFlowMultiplier = (decimal?)adl.MoneyFlowMultiplier;
                price.Volumes.AccumulationDistributionLine.MoneyFlowVolume = (decimal?)adl.MoneyFlowVolume;
                price.Volumes.AccumulationDistributionLine.Adl = (decimal?)adl.Adl;
                price.Volumes.AccumulationDistributionLine.AdlSma = (decimal?)adl.AdlSma;
            }

            index = 0;
            foreach (var cmf in prices.GetCmf(ChaikinMoneyFlow.DefaultLookbackPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Volumes.ChaikinMoneyFlow.MoneyFlowMultiplier = (decimal?)cmf.MoneyFlowMultiplier;
                price.Volumes.ChaikinMoneyFlow.MoneyFlowVolume = (decimal?)cmf.MoneyFlowVolume;
                price.Volumes.ChaikinMoneyFlow.Cmf = (decimal?)cmf.Cmf;
            }

            index = 0;
            foreach (var co in prices.GetChaikinOsc(ChaikinOscillator.DefaultFastPeriods, ChaikinOscillator.DefaultSlowPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Volumes.ChaikinOscillator.MoneyFlowMultiplier = (decimal?)co.MoneyFlowMultiplier;
                price.Volumes.ChaikinOscillator.MoneyFlowVolume = (decimal?)co.MoneyFlowVolume;
                price.Volumes.ChaikinOscillator.Adl = (decimal?)co.Adl;
                price.Volumes.ChaikinOscillator.Oscillator = (decimal?)co.Oscillator;
            }

            index = 0;
            foreach (var fi in prices.GetForceIndex(PriceVolumes.DefaultForceIndexLookbackPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Volumes.ForceIndex = (decimal?)fi.ForceIndex;
            }

            index = 0;
            foreach (var kvo in prices.GetKvo(KlingerVolumeOscillator.DefaultFastPeriods, KlingerVolumeOscillator.DefaultSlowPeriods, KlingerVolumeOscillator.DefaultSignalPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Volumes.KlingerVolumeOscillator.Oscillator = (decimal?)kvo.Oscillator;
                price.Volumes.KlingerVolumeOscillator.Signal = (decimal?)kvo.Signal;
            }

            index = 0;
            foreach (var mfi in prices.GetMfi(PriceVolumes.DefaultMoneyFlowIndexLookbackPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Volumes.MoneyFlowIndex = (decimal?)mfi.Mfi;
            }

            

            index = 0;
            foreach (var pvo in prices.GetPvo(PercentageVolumeOscillator.DefaultFastPeriods, PercentageVolumeOscillator.DefaultSlowPeriods, PercentageVolumeOscillator.DefaultSignalPeriods))
            {
                var price = prices[index];
                index++;
                if (price.TimeOpen < start)
                {
                    continue;
                }

                price.Volumes.PercentageVolumeOscillator.Pvo = (decimal?)pvo.Pvo;
                price.Volumes.PercentageVolumeOscillator.Signal = (decimal?)pvo.Signal;
                price.Volumes.PercentageVolumeOscillator.Histogram = (decimal?)pvo.Histogram;
            }
        }

        private Price[] GetPrices(BinanceContext context, string symbol, DateOnly start, DateOnly end, int historyHours, params Expression<Func<Price, FeatureContainer>>[] indicatorGroups)
        {
            var priceStart = new DateTimeOffset(start, TimeOnly.MinValue, TimeSpan.Zero);
            var dataStart = priceStart.AddHours(-historyHours);
            var dataEnd = new DateTimeOffset(end, TimeOnly.MaxValue, TimeSpan.Zero);
            var prices = context.Prices.AsQueryable();
            foreach(var include in indicatorGroups)
            {
                prices = prices.Include(include);
            }
            prices = prices
                .Where(x => x.Crypto.Symbol == symbol && x.TimeOpen >= dataStart && x.TimeOpen <= dataEnd)
                .OrderBy(x => x.TimeOpen);
                
            return prices.ToArray();
        }

        private void UpdateEndTime(Price[] prices, Func<Price, FeatureContainer> expression, Action<DateTimeOffset> setter)
        {
            for(var i = prices.Length - 1; i >= 0; i--)
            {
                var price = prices[i];
                var featureContainer = expression(price);
                if(featureContainer?.HasAllData() ?? false)
                {
                    setter(price.TimeOpen);
                    break;
                }
            }
        }

        private BinanceContext GetContext()
        {
            return _contextFactory.CreateDbContext();
        }
    }
}

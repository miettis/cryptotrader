using CryptoTrader.Data.Features;
using CryptoTrader.Data.Services;
using MathNet.Numerics.LinearAlgebra.Factorization;
using Microsoft.EntityFrameworkCore;
using OoplesFinance.StockIndicators.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using CryptoTrader.Data.Analyzers.Custom;

namespace CryptoTrader.Data.Analyzers
{
    public class AnalyzerService
    {
        public static JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            Converters = { new JsonStringEnumConverter() }
        };
        private readonly IDbContextFactory<BinanceContext> _contextFactory;
        private readonly IServiceProvider _serviceProvider;
        public AnalyzerService(IDbContextFactory<BinanceContext> contextFactory, IServiceProvider serviceProvider)
        {
            _contextFactory = contextFactory;
            _serviceProvider = serviceProvider;
        }
        public void Analyze(int cryptoId, DateTimeOffset start, DateTimeOffset end, Type[] includedAnalyzers = null)
        {
            var historyHours = 24 * 30;
            var context = GetContext();
            var indicators = context.Indicators
                .Include(x => x.Features)
                .ThenInclude(x => x.Output)
                .Include(x => x.Analyzer)
                .ToArray();

            var prices = GetPrices(context, cryptoId, start, end, historyHours);
            var crypto = context.Cryptos.First(x => x.Id == cryptoId);

            var open = prices.Select(x => (double)x.Open).ToArray();
            var high = prices.Select(x => (double)x.High).ToArray();
            var low = prices.Select(x => (double)x.Low).ToArray();
            var close = prices.Select(x => (double)x.Close).ToArray();
            var volume = prices.Select(x => (double)x.Volume).ToArray();
            var time = prices.Select(x => x.TimeOpen.DateTime).ToArray();

            var features = new Dictionary<string, List<object>>();

            foreach (var indicator in indicators.OrderBy(x => x.Analyzer.Order))
            {
                var type = typeof(AnalyzerBase).Assembly.GetType(indicator.Analyzer.Type);
                /*
                if(type == typeof(CandlestickAnalyzer))
                {
                    continue;
                }
                */
                if(includedAnalyzers != null && !includedAnalyzers.Contains(type))
                {
                    continue;
                }
                /*
                object[] parameters = null;
                if (type.BaseType.GetGenericTypeDefinition() == typeof(SecondOrderAnalyzerBase<>))
                {
                    parameters = new[] { context };
                }
                var analyzer = Activator.CreateInstance(type, parameters);
                */
                var analyzer = _serviceProvider.GetService(type);
                var settingsType = analyzer.GetType().BaseType.GetGenericArguments().First();
                var settings = JsonSerializer.Deserialize(indicator.Parameters, settingsType, JsonOptions);
                var analyzeMethod = type.GetMethod("Analyze");
                Dictionary<string, List<double>> result = new Dictionary<string, List<double>>();
                if (analyzer.GetType().BaseType.GetGenericTypeDefinition() == typeof(SkendrAnalyzerBase<>))
                {
                    var resultWithNulls = analyzeMethod.Invoke(analyzer, [prices, settings]) as Dictionary<string, List<double?>>;
                    foreach(var key in resultWithNulls.Keys)
                    {
                        result.Add(key, resultWithNulls[key].Select(x => x ?? double.NaN).ToList());
                    }
                }
                if (analyzer.GetType().BaseType.GetGenericTypeDefinition() == typeof(StockDataAnalyzerBase<>))
                {
                    var data = new StockData(open, high, low, close, volume, time);
                    result = analyzeMethod.Invoke(analyzer, [data, settings]) as Dictionary<string, List<double>>;
                }
                if (analyzer.GetType().BaseType.GetGenericTypeDefinition() == typeof(CustomAnalyzerBase<>))
                {
                    var resultWithNulls = analyzeMethod.Invoke(analyzer, [prices, settings]) as Dictionary<string, List<double?>>;
                    foreach (var key in resultWithNulls.Keys)
                    {
                        result.Add(key, resultWithNulls[key].Select(x => x ?? double.NaN).ToList());
                    }
                }
                if (analyzer.GetType().BaseType.GetGenericTypeDefinition() == typeof(SecondOrderAnalyzerBase<>))
                {
                    var resultWithNulls = analyzeMethod.Invoke(analyzer, [prices, settings]) as Dictionary<string, List<double?>>;
                    foreach (var key in resultWithNulls.Keys)
                    {
                        result.Add(key, resultWithNulls[key].Select(x => x ?? double.NaN).ToList());
                    }
                }
                foreach (var key in result.Keys)
                {
                    var feature = indicator.Features.First(x => x.Output.Key == key);

                    var priceIndex = 0;
                    foreach(var value in result[key])
                    {
                        var price = prices[priceIndex];
                        if(price.TimeOpen < start || price.TimeOpen > end)
                        {
                            priceIndex++;
                            continue;
                        }

                        price.Features = price.Features ?? new List<PriceFeature>();
                        var priceFeature = price.Features.FirstOrDefault(x => x.FeatureId == feature.Id);
                        if(priceFeature == null)
                        {
                            price.Features.Add(new PriceFeature 
                            {
                                Price = price,
                                FeatureId = feature.Id,
                                Value = value
                            });
                        }
                        else
                        {
                            priceFeature.Value = value;
                        }

                        priceIndex++;
                    }
                }
            }

            context.SaveChanges();
        }
        private BinanceContext GetContext()
        {
            return _contextFactory.CreateDbContext();
        }
        private Price[] GetPrices(BinanceContext context, int cryptoId, DateTimeOffset start, DateTimeOffset end, int historyHours)
        {
            var dataStart = start.AddHours(-historyHours);
            var dataEnd = end;
            var prices = context.Prices.Include(x => x.Features).AsQueryable();
            prices = prices
                .Where(x => x.CryptoId == cryptoId && x.TimeOpen >= dataStart && x.TimeOpen <= dataEnd)
                .OrderBy(x => x.TimeOpen);

            return prices.ToArray();
        }
    }
}

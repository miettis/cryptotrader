using Microsoft.EntityFrameworkCore;
using OoplesFinance.StockIndicators.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CryptoTrader.Data.Analyzers
{
    public abstract class AnalyzerBase
    {
    }
    public abstract class SkendrAnalyzerBase<TSettings> : AnalyzerBase
    {
        public abstract Dictionary<string, List<double?>> Analyze(Price[] prices, TSettings settings);
    }
    public abstract class StockDataAnalyzerBase<TSettings> : AnalyzerBase
    {
        public abstract Dictionary<string, List<double>> Analyze(StockData data, TSettings settings);
    }
    public abstract class CustomAnalyzerBase<TSettings> : AnalyzerBase
    {
        public abstract Dictionary<string, List<double?>> Analyze(Price[] prices, TSettings settings);
        public abstract string[] GetOutputs();
    }
    public abstract class SecondOrderAnalyzerBase<TSettings> : AnalyzerBase
    {
        protected readonly BinanceContext Context;
        protected SecondOrderAnalyzerBase(BinanceContext context)
        {
            Context = context;
        }
        public abstract Dictionary<string, List<double?>> Analyze(Price[] prices, TSettings settings);
        public abstract string[] GetOutputs();

        protected Indicator[] GetIndicators<TAnalyzer>()
        {
            var typeName = typeof(TAnalyzer).FullName;
            return Context.Indicators.AsNoTracking()
                .Include(x => x.Features)
                .ThenInclude(x => x.Output)
                .Include(x => x.Analyzer)
                .ThenInclude(x => x.Outputs)
                .Where(x => x.Analyzer.Type == typeName)
                .ToArray();
        }
        protected Indicator GetIndicator<TAnalyzer>(int id)
        {
            var typeName = typeof(TAnalyzer).FullName;
            return Context.Indicators.AsNoTracking()
                .Include(x => x.Features)
                .ThenInclude(x => x.Output)
                .Include(x => x.Analyzer)
                .ThenInclude(x => x.Outputs)
                .Where(x => x.Analyzer.Type == typeName && x.Id == id)
                .FirstOrDefault();
        }
        protected Indicator GetIndicator<TAnalyzer, TSettings>(Func<TSettings, bool> predicate)
        {
            var indicators = GetIndicators<TAnalyzer>();

            foreach (var indicator in indicators)
            {
                var settings = JsonSerializer.Deserialize<TSettings>(indicator.Parameters);
                if (predicate(settings))
                {
                    return indicator;
                }
            }

            return null;
        }
        protected Dictionary<long, double> GetFeatureValues(Price[] prices, int featureId)
        {
            var cryptoId = prices.First().CryptoId;
            var startTime = prices.First().TimeOpen;
            var endTime = prices.Last().TimeOpen;

            return Context.PriceFeatures.AsNoTracking()
                .Where(x => x.Price.CryptoId == cryptoId && x.Price.TimeOpen >= startTime && x.Price.TimeOpen <= endTime && x.FeatureId == featureId)
                .ToDictionary(x => x.PriceId, x => x.Value);
        }

        protected (Price Price, double FeatureValue)[] GetFeatureValuesZip(Price[] prices, int featureId)
        {
            var featureValues = GetFeatureValues(prices, featureId);
            var result = new List<(Price, double)>();
            foreach (var price in prices)
            {
                if (featureValues.TryGetValue(price.Id, out var featureValue))
                {
                    result.Add((price, featureValue));
                }
                else
                {
                    result.Add((price, double.NaN));
                }
            }
            return result.ToArray();
        }
    }
}

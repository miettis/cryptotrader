

using CryptoTrader.Data;
using CryptoTrader.Data.Analyzers;
using CryptoTrader.Data.Analyzers.Custom;
using MathNet.Numerics.LinearAlgebra.Factorization;
using CryptoTrader.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using OoplesFinance.StockIndicators.Models;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using DocumentFormat.OpenXml.Drawing;

var includedCryptoIds = new[] { 1, 2, 5, 6, 10 };

var services = new ServiceCollection();
services.AddDbContextFactory<BinanceContext>(x => x
    .UseNpgsql(@"")
    .UseSnakeCaseNamingConvention());

services.AddSingleton(new PythonService("http://192.168.70.1:9002"));

var analyzerTypes = typeof(AnalyzerBase).Assembly.GetTypes().Where(x => typeof(AnalyzerBase).IsAssignableFrom(x) && !x.IsAbstract).ToArray();
foreach (var analyzerType in analyzerTypes)
{
    services.AddTransient(analyzerType);
}


var serviceProvider = services.BuildServiceProvider();
var contextFactory = serviceProvider.GetRequiredService<IDbContextFactory<BinanceContext>>();


var jsonOptions = AnalyzerService.JsonOptions;

void UpdateAnalyzers()
{
    var cryptoId = 1;
    var start = new DateTimeOffset(2021, 8, 13, 6, 0, 0, TimeSpan.Zero);
    var end = new DateTimeOffset(2021, 9, 29, 6, 0, 0, TimeSpan.Zero);
    var context = contextFactory.CreateDbContext();

    var prices = context.Prices.Where(x => x.CryptoId == cryptoId && x.TimeOpen >= start && x.TimeOpen <= end).ToArray();
    var open = prices.Select(x => (double)x.Open).ToArray();
    var high = prices.Select(x => (double)x.High).ToArray();
    var low = prices.Select(x => (double)x.Low).ToArray();
    var close = prices.Select(x => (double)x.Close).ToArray();
    var volume = prices.Select(x => (double)x.Volume).ToArray();
    var time = prices.Select(x => x.TimeOpen.DateTime).ToArray();

    var analyzerTypes = typeof(AnalyzerBase).Assembly.GetTypes().Where(x => typeof(AnalyzerBase).IsAssignableFrom(x) && !x.IsAbstract).ToArray();

    var analyzers = context.Analyzers.Include(x => x.Outputs).ToList();

    foreach(var analyzerType in analyzerTypes)
    {
        /*
        object[] parameters = null;
        if (analyzerType.BaseType.GetGenericTypeDefinition() == typeof(SecondOrderAnalyzerBase<>))
        {
            parameters = new[] { context };
        }
        var instance = Activator.CreateInstance(analyzerType, parameters);
        */
        var instance = serviceProvider.GetService(analyzerType);
        var settingsType = instance.GetType().BaseType.GetGenericArguments().First();
        var settings = Activator.CreateInstance(settingsType);
        var analyzeMethod = analyzerType.GetMethod("Analyze");

        Dictionary<string, List<double>> result = new Dictionary<string, List<double>>();
        if (instance.GetType().BaseType.GetGenericTypeDefinition() == typeof(SkendrAnalyzerBase<>))
        {
            var resultWithNulls = analyzeMethod.Invoke(instance, [prices, settings]) as Dictionary<string, List<double?>>;
            foreach (var key in resultWithNulls.Keys)
            {
                result.Add(key, resultWithNulls[key].Select(x => x ?? double.NaN).ToList());
            }
        }
        if (instance.GetType().BaseType.GetGenericTypeDefinition() == typeof(StockDataAnalyzerBase<>))
        {
            var data = new StockData(open, high, low, close, volume, time);
            result = analyzeMethod.Invoke(instance, [data, settings]) as Dictionary<string, List<double>>;
        }
        if (instance.GetType().BaseType.GetGenericTypeDefinition() == typeof(CustomAnalyzerBase<>))
        {
            var getOutputsMethod = analyzerType.GetMethod("GetOutputs");
            var outputs = getOutputsMethod.Invoke(instance, null) as string[];
            foreach (var output in outputs)
            {
                result.Add(output, new List<double>());
            }
        }
        if (instance.GetType().BaseType.GetGenericTypeDefinition() == typeof(SecondOrderAnalyzerBase<>))
        {
            var getOutputsMethod = analyzerType.GetMethod("GetOutputs");
            var outputs = getOutputsMethod.Invoke(instance, null) as string[];
            foreach(var output in outputs)
            {
                result.Add(output, new List<double>());
            }
        }

        var analyzer = analyzers.FirstOrDefault(x => x.Type == analyzerType.FullName);
        if(analyzer == null)
        {
            analyzer = new Analyzer
            {
                Type = analyzerType.FullName,
                Outputs = new List<AnalyzerOutput>()
            };

            context.Analyzers.Add(analyzer);
        }

        foreach(var key in result.Keys)
        {
            if(analyzer.Outputs.Any(x => x.Key == key))
            {
                continue;
            }

            analyzer.Outputs.Add(new AnalyzerOutput 
            {
                Analyzer = analyzer,
                Key = key
            });
        }


    }

    context.SaveChanges();
}
void UpdateExampleIndicators()
{
    var context = contextFactory.CreateDbContext();
    var analyzers = context.Analyzers.Include(x => x.Outputs).ToList();
    var indicators = context.Indicators.Include(x => x.Features).ToList();

    foreach(var analyzer in analyzers)
    {
        var type = typeof(AnalyzerBase).Assembly.GetType(analyzer.Type);
        var settingsType = type.BaseType.GetGenericArguments().First();
        var settings = Activator.CreateInstance(settingsType);

        var indicator = indicators.FirstOrDefault(x => x.AnalyzerId == analyzer.Id);
        if(indicator == null)
        {
            indicator = new CryptoTrader.Data.Analyzers.Indicator
            {
                AnalyzerId = analyzer.Id,
                Name = type.Name.Replace("Analyzer", "") + " Example",
                Parameters = JsonSerializer.Serialize(settings, jsonOptions),
                Features = new List<CryptoTrader.Data.Analyzers.Feature>()
            };
            context.Indicators.Add(indicator);
        }

        foreach (var output in analyzer.Outputs)
        {
            if(indicator.Features.Any(x => x.OutputId == output.Id))
            {
               continue;
            }
            indicator.Features.Add(new CryptoTrader.Data.Analyzers.Feature
            {
                Indicator = indicator,
                OutputId = output.Id
            });
        }
    }

    context.SaveChanges();
}
void CreateAdditionalIndicators()
{
    var indicators = new(Type Type, object Setting, string Name)[]
    {
        (typeof(SmaAnalyzer), new SmaAnalyzer.Settings { LookbackPeriods = 120 }, "SMA 120h" )
    };

    var context = contextFactory.CreateDbContext();

    foreach (var indicator in indicators) 
    {
        var existing = context.Indicators.Where(x => x.Analyzer.Type == indicator.Type.FullName).ToList();
        var matchFound = false;
        foreach(var ind in existing)
        {
            var settings = JsonSerializer.Deserialize(ind.Parameters, indicator.Setting.GetType(), jsonOptions);
            var propertiesMatch = true;
            foreach(var property in indicator.Setting.GetType().GetProperties())
            {
                var value = property.GetValue(indicator.Setting, null);
                var value2 = property.GetValue(settings, null);
                propertiesMatch = propertiesMatch && value == value2;
            }
            if (propertiesMatch)
            {
                matchFound = true;
            }
        }

        if (!matchFound)
        {
            var analyzer = context.Analyzers.Include(x => x.Outputs).First(x => x.Type == indicator.Type.FullName);
            var newIndicator = new Indicator
            {
                Name = indicator.Name,
                AnalyzerId = analyzer.Id,
                Parameters = JsonSerializer.Serialize(indicator.Setting, jsonOptions),
                Features = new List<CryptoTrader.Data.Analyzers.Feature>()
            };

            foreach(var output in analyzer.Outputs)
            {
                newIndicator.Features.Add(new CryptoTrader.Data.Analyzers.Feature 
                { 
                    Indicator = newIndicator,
                    OutputId = output.Id,
                });
            }

            context.Indicators.Add(newIndicator);
            context.SaveChanges();
        }
    }
}

void UpdateStreaks()
{
    var context = contextFactory.CreateDbContext();
    var cryptos = context.Cryptos.OrderBy(x => x.Rank).ToList();
    foreach (var crypto in cryptos)
    {
        var timestamps = context.Prices.Where(x => x.CryptoId == crypto.Id).OrderBy(x => x.TimeOpen).Select(x => x.TimeOpen).ToList();
        if (timestamps.Count == 0)
        {
            continue;
        }
        var streak = new PriceStreak
        {
            CryptoId = crypto.Id,
            Count = 0,
            StartTime = timestamps[0],
            EndTime = timestamps[0]
        };
        var streaks = new List<PriceStreak>();
        for (var i = 1; i < timestamps.Count; i++)
        {
            var timestamp = timestamps[i];
            if (timestamp == streak.EndTime.AddHours(1))
            {
                streak.EndTime = timestamp;
                continue;
            }
            if (timestamp > streak.EndTime.AddHours(1))
            {
                streak.Count = (int)(streak.EndTime - streak.StartTime).TotalHours + 1;
                streaks.Add(streak);
                streak = new PriceStreak
                {
                    CryptoId = crypto.Id,
                    Count = 0,
                    StartTime = timestamp,
                    EndTime = timestamp
                };
            }
        }
        streak.Count = (int)(streak.EndTime - streak.StartTime).TotalHours + 1;
        streaks.Add(streak);

        var existingStreaks = context.PriceStreaks.Where(x => x.CryptoId == crypto.Id).ToList();
        context.PriceStreaks.RemoveRange(existingStreaks);
        context.PriceStreaks.AddRange(streaks);

        context.SaveChanges();
    }
}
async Task ImportMissingPrices()
{
    var import = new BinanceImport(contextFactory, NullLogger.Instance, new BinanceImportConfig { DailyDirectory = "daily"});
    var context = contextFactory.CreateDbContext();
    var cryptos = context.Cryptos.OrderBy(x => x.Rank).ToList();
    foreach(var crypto in cryptos)
    {
        var streaks = context.PriceStreaks.Where(x => x.CryptoId == crypto.Id && x.StartTime.Year >= 2023).OrderBy(x => x.StartTime).ToList();
        for(var i = 1; i < streaks.Count; i++)
        {
            var current = streaks[i];
            var previous = streaks[i - 1];

            var startTime = previous.EndTime.AddHours(1);
            var endTime = current.StartTime.AddHours(-1);

            Console.WriteLine($"Importting {crypto.Symbol} {startTime} - {endTime}");

            await import.FetchDaily(crypto.Symbol, "1h", startTime.GetDate(), endTime.GetDate());
            await import.ImportDaily("1h", DateOnly.FromDateTime(startTime.DateTime), DateOnly.FromDateTime(endTime.DateTime), [crypto.Symbol]);
        }
    }
}
async Task ImportLatestPrices()
{
    var import = new BinanceImport(contextFactory, NullLogger.Instance, new BinanceImportConfig { DailyDirectory = "daily" });
    var context = contextFactory.CreateDbContext();
    var cryptos = context.Cryptos.OrderBy(x => x.Rank).ToList();
    foreach (var crypto in cryptos)
    {
        var streak = context.PriceStreaks.Where(x => x.CryptoId == crypto.Id).OrderByDescending(x => x.StartTime).FirstOrDefault();
        if(streak != null)
        {
            var startTime = streak.EndTime.AddHours(1);
            var endTime = DateTimeOffset.UtcNow.StartOfDay().AddHours(-1);
            if(startTime < endTime)
            {
                Console.WriteLine($"Importting {crypto.Symbol} {startTime} - {endTime}");

                await import.FetchDaily(crypto.Symbol, "1h", startTime.GetDate(), endTime.GetDate());
                await import.ImportDaily("1h", DateOnly.FromDateTime(startTime.DateTime), DateOnly.FromDateTime(endTime.DateTime), [crypto.Symbol]);
            }
            
        }
    }
}
static IEnumerable<Streak> GetStreaks()
{
    var streaks = new List<Streak>();
    var files = Directory.GetFiles(@"C:\Docker\crypto_streaks", "*_streaks.json");
    foreach (var file in files)
    {
        var json = File.ReadAllText(file);
        var cryptoStreaks = JsonSerializer.Deserialize<List<Streak>>(json);
        streaks.AddRange(cryptoStreaks);
    }
    return streaks;
}
void RunAnalyzers()
{
    var service = new AnalyzerService(contextFactory, serviceProvider);
    var context = contextFactory.CreateDbContext();
    var cryptos = context.Cryptos.OrderBy(x => x.Rank).ToList();
    foreach(var crypto in cryptos)
    {
        var streaks = context.PriceStreaks.Where(x => x.CryptoId == crypto.Id).OrderBy(x => x.StartTime).ToList();

        foreach(var streak in streaks)
        {
            var sw = Stopwatch.StartNew();
            service.Analyze(streak.CryptoId, streak.StartTime, streak.EndTime);
            sw.Stop();
            var hours = (int)(streak.EndTime - streak.StartTime).TotalHours + 1;
            Console.WriteLine($"Analyzed crypto {streak.CryptoId} {hours} hours in {sw.Elapsed.TotalSeconds} seconds");
        }
    }
}
void RunAnalyzersLastStreak()
{
    var service = new AnalyzerService(contextFactory, serviceProvider);
    var context = contextFactory.CreateDbContext();
    var cryptos = context.Cryptos.OrderBy(x => x.Rank).ToList();
    foreach (var crypto in cryptos)
    {
        if (!includedCryptoIds.Contains(crypto.Id))
        {
            continue;
        }
        var streak = context.PriceStreaks.Where(x => x.CryptoId == crypto.Id).OrderByDescending(x => x.StartTime).FirstOrDefault();
        if (streak != null)
        {
            var hours = (int)(streak.EndTime - streak.StartTime).TotalHours + 1;

            Console.WriteLine($"Analyzing crypto {streak.CryptoId} {hours} hours");

            var sw = Stopwatch.StartNew();
            service.Analyze(streak.CryptoId, streak.StartTime, streak.EndTime);
            sw.Stop();
            
            Console.WriteLine($"Analyzed crypto {streak.CryptoId} {hours} hours in {sw.Elapsed.TotalSeconds} seconds");
        }
    }
}

void RunAnalyzersRecent(int days)
{
    var service = new AnalyzerService(contextFactory, serviceProvider);
    var context = contextFactory.CreateDbContext();
    var cryptos = context.Cryptos.Where(x => includedCryptoIds.Contains(x.Id)).OrderBy(x => x.Rank).ToList();
    foreach (var crypto in cryptos)
    {
        var endTime = DateTime.UtcNow;
        var startTime = endTime.AddDays(-days);

        Console.WriteLine($"Analyzing crypto");

        var sw = Stopwatch.StartNew();
        service.Analyze(crypto.Id, startTime, endTime);
        sw.Stop();

        Console.WriteLine($"Analyzed crypto {crypto.Id} in {sw.Elapsed.TotalSeconds} seconds");
        
    }
}

void RunSingleAnalyzer<TAnalyzer>()
{
    var service = new AnalyzerService(contextFactory, serviceProvider);
    var context = contextFactory.CreateDbContext();
    var cryptos = context.Cryptos.OrderBy(x => x.Rank).ToList();
    foreach (var crypto in cryptos)
    {
        if(crypto.Id != 5 && crypto.Id != 10)
        {
            continue;
        }
        var streaks = context.PriceStreaks.Where(x => x.CryptoId == crypto.Id).OrderBy(x => x.StartTime).ToList();

        foreach (var streak in streaks)
        {
            if (streak.Count < 20000)
            {
                continue;
            }

            var hours = (int)(streak.EndTime - streak.StartTime).TotalHours + 1;
            Console.Write($"Analyzing crypto {streak.CryptoId} {crypto.Name} {hours} hours {streak.StartTime} - {streak.EndTime}");

            
            var sw = Stopwatch.StartNew();
            service.Analyze(streak.CryptoId, streak.StartTime, streak.EndTime, [typeof(TAnalyzer)]);
            sw.Stop();
            
            Console.WriteLine($" took {sw.Elapsed.TotalSeconds} seconds");
        }
    }
}
async Task CalculateStats()
{
    var start = DateTimeOffset.UtcNow.AddYears(-2);
    var end = DateTimeOffset.UtcNow;
    var context = contextFactory.CreateDbContext();
    var stats = await context.GetCryptoStatisticsAsync(null, start, end);
    var existing = context.CryptoStatistics.FirstOrDefault(x => x.CryptoId == CryptoStatistics.StatsCryptoId);
    if (existing != null) 
    {
        context.CryptoStatistics.Remove(existing);
    }
   
    await context.CryptoStatistics.AddAsync(stats);
    
    await context.SaveChangesAsync();
}

void ExportFeatureInfo()
{
    var context = contextFactory.CreateDbContext();
    var analyzers = context.Analyzers.Include(x => x.Outputs).ToList();
    var indicators = context.Indicators.Include(x => x.Features).ThenInclude(x => x.Output).Include(x => x.Analyzer).ThenInclude(x => x.Outputs).ToList();

    var filename = "features.xlsx";
    if(File.Exists(filename))
    {
        File.Delete(filename);
    }

    var excel = new ClosedXML.Excel.XLWorkbook();
    var sheet = excel.AddWorksheet("Features");
    var row = 1;
    sheet.Cell(row, 1).Value = "Id";
    sheet.Cell(row, 2).Value = "Technical Indicator / Output";
    sheet.Cell(row, 3).Value = "Parameters / Output Type";

    row++;
    foreach (var indicator in indicators)
    {
        var name = indicator.Analyzer.Type.Split('.').Last().Replace("Analyzer", "");
        name = string.Join("", name.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
        var parameters = indicator.Parameters;

        var analyzerType = typeof(AnalyzerBase).Assembly.GetType(indicator.Analyzer.Type);

        Dictionary<string, List<double>> result = new Dictionary<string, List<double>>();
        if (analyzerType.BaseType.GetGenericTypeDefinition() == typeof(SkendrAnalyzerBase<>))
        {
            name += " *";
        }
        else if (analyzerType.BaseType.GetGenericTypeDefinition() == typeof(StockDataAnalyzerBase<>))
        {
            name += " **";
        }
        else
        {
            name += " ***";
        }

        sheet.Cell(row, 2).Value = name;
        sheet.Cell(row, 3).Value = parameters;

        sheet.Range(row,1, row, 3).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;

        row++;

        foreach(var feature in indicator.Features)
        {
            var id = feature.Id;
            var key = feature.Output.Key;
            var type = feature.Output.IsClass ? "boolean" : "numeric";

            sheet.Cell(row, 1).Value = id;
            sheet.Cell(row, 2).Value = key;
            sheet.Cell(row, 3).Value = type;

            row++;
        }
    }

    excel.SaveAs(filename);
}

//ExportFeatureInfo();
//await ImportMissingPrices();
//UpdateStreaks();
RunSingleAnalyzer<ProfitRankAnalyzer>();
//RunAnalyzersLastStreak();
//RunAnalyzersRecent(90);
//UpdateAnalyzers();
//UpdateExampleIndicators();
//CreateAdditionalIndicators();
//await CalculateStats();
;

class Streak
{
    public int CryptoId { get; set; }
    public int Hours { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
}

/*
var typeName = typeof(EhlersAdaptiveCyberCycleAnalyzer).FullName;
var type = typeof(AnalyzerBase).Assembly.GetType(typeName);
var instance = Activator.CreateInstance(type);
var settingsType = instance.GetType().BaseType.GetGenericArguments().First(); 
var settings = Activator.CreateInstance(settingsType);
var analyzeMethod = type.GetMethod("Analyze");

if (instance.GetType().BaseType == typeof(SkendrAnalyzerBase<>))
{
    var result = analyzeMethod.Invoke(instance, new[] { null, settings });
    Console.WriteLine(result);
}
if(instance.GetType().BaseType.GetGenericTypeDefinition() == typeof(StockDataAnalyzerBase<>))
{
    var result = analyzeMethod.Invoke(instance, new[] { null, settings });
    Console.WriteLine(result);
}
*/
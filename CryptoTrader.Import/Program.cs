using Binance.Net.Clients;
using CryptoExchange.Net.Authentication;
using CryptoTrader.Data;
using CryptoTrader.Data.Extensions;
using CryptoTrader.Data.Features;
using CryptoTrader.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using CryptoTrader.Data;

var importConfig = new BinanceImportConfig
{
    DailyDirectory = @"C:\Users\mkode\Downloads\crypto\daily",
    MonthlyDirectory = @"C:\Users\mkode\Downloads\crypto\monthly",
    DeleteFileAfterInsert = false,
    SleepAfterFetch = 5000,
    UpdateDataStartTime = false
};
using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
var logger = loggerFactory.CreateLogger<Program>();


var symbols = new[] { "EURUSDT" };
var useLocalPostgreSQL = true;
var services = new ServiceCollection();
services.AddDbContextFactory<BinanceContext>(x => x
    .UseNpgsql(@"")
    .UseSnakeCaseNamingConvention());
services.AddSingleton(new PythonService("http://localhost:80"));
services.AddTransient<FeatureCalculation>();
var serviceProvider = services.BuildServiceProvider();

var contextFactory = serviceProvider.GetRequiredService<IDbContextFactory<BinanceContext>>();

var context = contextFactory.CreateDbContext();
//symbols = context.Cryptos.Where(x => !x.Symbol.StartsWith("USD")).Select(x => x.Symbol).ToArray();
var processed = context.Prices.Where(x => x.TimeOpen == new DateTimeOffset(2014,1,30,23,0,0,TimeSpan.Zero) && x.MA.SMA24.Sma != null).Select(x => x.Crypto.Symbol).ToArray();
//symbols = symbols.Where(x => !processed.Contains(x)).ToArray();

void ImportCryptos()
{
    var srcContext = BinanceContextFactory.CreateDbContextSqlServer();
    var targetContext = contextFactory.CreateDbContext();

    foreach (var crypto in srcContext.Cryptos.OrderBy(x => x.Rank).ToList())
    {
        var copy = new Crypto
        {
            Name = crypto.Name,
            Symbol = crypto.Symbol.AsSymbolPair(),
            Rank = crypto.Rank,
            Times = new Timestamps
            {
                StartData = crypto.Times.StartData.Offset.TotalSeconds > 0 ? crypto.Times.StartData.ToUniversalTime() : crypto.Times.StartData,
                Start = crypto.Times.Start.Offset.TotalSeconds > 0 ? crypto.Times.Start.ToUniversalTime() : crypto.Times.Start
            }
            
        };

        targetContext.Cryptos.Add(copy);
    }

    targetContext.SaveChanges();

}
void ImportHourDataFromMonthlyFiles()
{
    var context = contextFactory.CreateDbContext();
    var import = new BinanceImport(contextFactory, logger, importConfig);
    foreach(var symbol in symbols)
    {
        var crypto = context.Cryptos.First(x => x.Symbol == symbol);
        import.ImportMonthly("1h", crypto.Times.StartData.GetDate(), DateTimeOffset.UtcNow.StartOfDay().AddMilliseconds(-1).GetDate(), (string[])(new[] { symbol }));
    }
    
}
void ImportHourDataFromDailyFiles()
{
    var context = contextFactory.CreateDbContext();
    var import = new BinanceImport(contextFactory, logger, importConfig);
    foreach (var symbol in symbols)
    {
        var crypto = context.Cryptos.First(x => x.Symbol == symbol);
        import.ImportDaily("1h", new DateOnly(2024,1,1), DateTimeOffset.UtcNow.StartOfDay().AddMilliseconds(-1).GetDate(), (string[])(new[] { symbol }));
    }

}
void ImportMinuteDataFromMonthlyFiles()
{
    var context = contextFactory.CreateDbContext();
    var import = new BinanceImport(contextFactory, logger, importConfig);
    foreach (var symbol in symbols)
    {
        var crypto = context.Cryptos.First(x => x.Symbol == symbol);
        import.ImportMonthly("1m", crypto.Times.StartData.GetDate(), DateTimeOffset.UtcNow.StartOfDay().AddMilliseconds(-1).GetDate(), (string[])(new[] { symbol }));
    }

}
void ImportMinuteDataFromDailyFiles()
{
    var context = contextFactory.CreateDbContext();
    var import = new BinanceImport(contextFactory, logger, importConfig);
    foreach (var symbol in symbols)
    {
        var crypto = context.Cryptos.First(x => x.Symbol == symbol);
        import.ImportDaily("1m", new DateOnly(2024, 1, 1), DateTimeOffset.UtcNow.StartOfDay().AddMilliseconds(-1).GetDate(), (string[])(new[] { symbol }));
    }

}
async Task FetchHourDataMonthly()
{
    var import = new BinanceImport(contextFactory, logger, importConfig);
    var endOfPreviousMonth = DateTimeOffset.UtcNow.StartOfMonth().AddMilliseconds(-1);
    foreach (var symbol in symbols)
    {
        await import.FetchMonthly(symbol, "1h", new DateOnly(2020, 1, 1), DateOnly.FromDateTime(endOfPreviousMonth.DateTime));
    }
}
async Task FetchHourDataDaily()
{
    var import = new BinanceImport(contextFactory, logger, importConfig);
    var startOfMonth = DateTimeOffset.UtcNow.StartOfMonth();
    foreach (var symbol in symbols)
    {
        var crypto = context.Cryptos.FirstOrDefault(x => x.Symbol == symbol);
        var startTime = crypto.Times.EndData ?? startOfMonth;

        await import.FetchDaily(symbol, "1h", DateOnly.FromDateTime(startTime.DateTime), DateTimeOffset.UtcNow.StartOfDay().AddMilliseconds(-1).GetDate());
    }
}
async Task FetchMinuteData()
{
    var import = new BinanceImport(contextFactory, logger, importConfig);
    foreach (var symbol in symbols)
    {
        await import.FetchDaily(symbol, "1m", new DateOnly(2024, 1, 1), DateTimeOffset.UtcNow.StartOfDay().AddMilliseconds(-1).GetDate());
    }
}

async Task CalculateStatistics()
{
    var statCryptoIds = context.CryptoStatistics.Select(x => x.CryptoId).Distinct().ToArray();
    var cryptos = await context.Cryptos.AsNoTracking().OrderBy(x => x.Id).ToListAsync();
    foreach (var crypto in cryptos)
    {
        if(statCryptoIds.Contains(crypto.Id))
        {
            continue;
        }
        /*
        for (var year = crypto.Times.StartData.Year; year <= 2023; year++)
        {
            var start = new DateTimeOffset(year, 1, 1, 0 ,0, 0, TimeSpan.Zero);
            var end = start.AddYears(1).AddMilliseconds(-1);

            try
            {
                var context2 = contextFactory.CreateDbContext();
                var stats = await context2.GetCryptoStatisticsAsync(crypto.Id, start, end);
                await context2.CryptoStatistics.AddAsync(stats);
                await context2.SaveChangesAsync();
            }
            catch(Exception ex) 
            {
                ;
            }
        }
        */

        var start = crypto.Times.StartData;
        var end = DateTimeOffset.UtcNow;

        try
        {
            var context2 = contextFactory.CreateDbContext();
            var stats = await context2.GetCryptoStatisticsAsync(crypto.Id, start, end);
            await context2.CryptoStatistics.AddAsync(stats);
            await context2.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            ;
        }
        
    }
}
void UpdateHourPeaks()
{
    using var scope = serviceProvider.CreateScope();
    var calc = scope.ServiceProvider.GetRequiredService<FeatureCalculation>();
    var cryptos = context.Cryptos.AsNoTracking().OrderBy(x => x.Id).ToList();
    foreach (var crypto in cryptos)
    {
        try
        {
            calc.UpdatePeaks(crypto.Symbol, crypto.Times.StartData.GetDate(), DateTimeOffset.UtcNow.StartOfDay().AddMilliseconds(-1).GetDate());
        }
        catch
        {
            ;
        }
    }
}

void UpdateFeatures()
{
    using var scope = serviceProvider.CreateScope();
    var calc = scope.ServiceProvider.GetRequiredService<FeatureCalculation>();
    var cryptos = context.Cryptos.AsNoTracking().OrderBy(x => x.Id).ToList();
    foreach (var crypto in cryptos)
    {
        //Console.WriteLine($"Processing {crypto.Symbol}");
        try
        {
            calc.UpdateCandleSticks(crypto.Symbol, crypto.Times.StartData.GetDate(), DateTimeOffset.UtcNow.StartOfDay().AddMilliseconds(-1).GetDate());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update candlesticks {crypto.Symbol} | {ex.Message}");
        }
 
        try
        {
            calc.UpdateCycles(crypto.Symbol, crypto.Times.StartData.GetDate(), DateTimeOffset.UtcNow.StartOfDay().AddMilliseconds(-1).GetDate());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update cycles {crypto.Symbol} | {ex.Message}");
        }
        try
        {
            calc.UpdateOtherIndicators(crypto.Symbol, crypto.Times.StartData.GetDate(), DateTimeOffset.UtcNow.StartOfDay().AddMilliseconds(-1).GetDate());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update misc features {crypto.Symbol} | {ex.Message}");
        }
        try
        {
            calc.UpdateMomentum(crypto.Symbol, crypto.Times.StartData.GetDate(), DateTimeOffset.UtcNow.StartOfDay().AddMilliseconds(-1).GetDate());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update momentum {crypto.Symbol} | {ex.Message}");
        }
        try
        {
            calc.UpdateMovingAverages(crypto.Symbol, crypto.Times.StartData.GetDate(), DateTimeOffset.UtcNow.StartOfDay().AddMilliseconds(-1).GetDate());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update moving averages {crypto.Symbol} | {ex.Message}");
        }
        try
        {
            calc.UpdatePeaks(crypto.Symbol, crypto.Times.StartData.GetDate(), DateTimeOffset.UtcNow.StartOfDay().AddMilliseconds(-1).GetDate());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update peaks {crypto.Symbol} | {ex.Message}");
        }
        try
        {
            calc.UpdateReturns(crypto.Symbol, crypto.Times.StartData.GetDate(), DateTimeOffset.UtcNow.StartOfDay().AddMilliseconds(-1).GetDate());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update returns {crypto.Symbol} | {ex.Message}");
        }
        try
        {
            calc.UpdateSlopes(crypto.Symbol, crypto.Times.StartData.GetDate(), DateTimeOffset.UtcNow.StartOfDay().AddMilliseconds(-1).GetDate());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update slopes {crypto.Symbol} | {ex.Message}");
        }
        try
        {
            calc.UpdateTrends(crypto.Symbol, crypto.Times.StartData.GetDate(), DateTimeOffset.UtcNow.StartOfDay().AddMilliseconds(-1).GetDate());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update trends {crypto.Symbol} | {ex.Message}");
        }
        try
        {
            calc.UpdateVolatilities(crypto.Symbol, crypto.Times.StartData.GetDate(), DateTimeOffset.UtcNow.StartOfDay().AddMilliseconds(-1).GetDate());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update volatilities {crypto.Symbol} | {ex.Message}");
        }
        try
        {
            calc.UpdateVolumes(crypto.Symbol, crypto.Times.StartData.GetDate(), DateTimeOffset.UtcNow.StartOfDay().AddMilliseconds(-1).GetDate());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update volumes {crypto.Symbol} | {ex.Message}");
        }
    }
}

async Task GenerateModels()
{
    using var scope = serviceProvider.CreateScope();
    var pred = scope.ServiceProvider.GetRequiredService<PythonService>();
    var cryptos = context.Cryptos.Include(x => x.Models).AsNoTracking().OrderBy(x => x.Id).ToList();
    foreach(var crypto in cryptos)
    {
        var startTime = crypto.Times.StartData;
        //var startTime = crypto.Times.StartData.AddMonths(18);
        var endTime = new DateTimeOffset(2024, 2, 1, 0, 0, 0, TimeSpan.Zero).AddMilliseconds(-1);
        try
        {
            var models1 = await pred.TrainClassifiers(crypto.Id, startTime, endTime);
            var models2 = await pred.TrainRegressors(crypto.Id, startTime, endTime);
            var context2 = contextFactory.CreateDbContext();
            foreach (var model in models1.Union(models2))
            {
                await context2.CryptoModels.AddAsync(new CryptoModel
                {
                    CryptoId = crypto.Id,
                    StartTime = startTime,
                    EndTime = endTime,
                    Output = model.Output,
                    ModelName = model.ModelName,
                    Accuracy = model.Accuracy,
                    Samples = model.Samples,
                    Created = DateTimeOffset.UtcNow
                });
                
            }
            await context2.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Failed to generate models {crypto.Symbol} | {ex.Message}");
        }
    }
}
async Task Predict()
{
    using var scope = serviceProvider.CreateScope();
    var pred = scope.ServiceProvider.GetRequiredService<PythonService>();
    var cryptos = context.Cryptos.Include(x => x.Models).AsNoTracking().OrderBy(x => x.Id).ToList();
    foreach (var crypto in cryptos)
    {
        var context2 = contextFactory.CreateDbContext();
        var models = crypto.Models.ToArray();    
        var time = new DateTimeOffset(2024, 2, 8, 0, 0, 0, TimeSpan.Zero);
        while(time.Date <= new DateTime(2024, 2, 27, 7, 0, 0))
        {
            try
            {
                var predictions = await pred.Predict(crypto.Id, time, models);
                var prediction = new PricePrediction
                {
                    CryptoId = crypto.Id,
                    TimeOpen = time.AddHours(1),
                    Created = DateTimeOffset.UtcNow
                };
                foreach (var kvp in predictions.Predictions)
                {
                    var parts = kvp.Key.Split('_');
                    if (kvp.Key.StartsWith("price_return", StringComparison.OrdinalIgnoreCase))
                    {
                        var intervalName = parts[2];
                        var rank = parts[3];

                        var intervalProperty = typeof(PricePrediction).GetProperties().FirstOrDefault(x => x.Name.Equals(intervalName, StringComparison.OrdinalIgnoreCase));
                        var rankProperty = typeof(PredictionRank).GetProperties().FirstOrDefault(x => x.Name.Equals(rank, StringComparison.OrdinalIgnoreCase));

                        var intervalRanks = intervalProperty.GetValue(prediction);
                        rankProperty.SetValue(intervalRanks, (int)kvp.Value);
                    }
                    else
                    {
                        var propertyName = parts[1];
                        var property = typeof(PricePrediction).GetProperties().FirstOrDefault(x => x.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
                        property.SetValue(prediction, kvp.Value);
                    }
                }   
                await context2.Predictions.AddAsync(prediction);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to predict {crypto.Symbol} {time} | {ex.Message}");
            }

            time = time.AddHours(1);
        }
        await context2.SaveChangesAsync();
    }
}

async Task UpdateCryptos()
{
    BinanceRestClient.SetDefaultOptions(options =>
    {
        var apiKey = "";
        var secret = "";
        //options.ApiCredentials = new ApiCredentials("", ""); // <- Provide you API key/secret in these fields to retrieve data related to your account
        options.ApiCredentials = new ApiCredentials(apiKey, secret); // <- Provide you API key/secret in these fields to retrieve data related to your account

    });
    var cryptos = context.Cryptos.ToList();
    var client = new BinanceRestClient();
    var result = await client.SpotApi.ExchangeData.GetExchangeInfoAsync();
    if (result.Success)
    {
        var symbols = result.Data.Symbols.Where(x => x.QuoteAsset == "USDT" && x.Status == Binance.Net.Enums.SymbolStatus.Trading).ToArray();
        foreach (var symbol in symbols)
        {
            if (symbol.QuoteAsset != "USDT")
            {
                continue;
            }
            if(symbol.Status != Binance.Net.Enums.SymbolStatus.Trading)
            {
                continue;
            }
            var crypto = cryptos.Where(x => x.Symbol == symbol.Name).FirstOrDefault();
            if(crypto == null)
            {
                crypto = new Crypto
                {
                    Name = symbol.Name,
                    Symbol = symbol.Name,
                    Rank = 100,
                    MaxPurchase = 200,
                    MaxTotal = 300m,
                    Times = new Timestamps
                    {
                        StartData = DateTimeOffset.UtcNow,
                        Start = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)
                    }
                };
                context.Cryptos.Add(crypto);
            }
            ;
        }

        context.SaveChanges();
    }
}

async Task MatchOrders()
{
    var orders = context.Orders.Where(x => x.Status == OrderStatus.Filled).ToList();
    var relations = new List<OrderRelation>();
    foreach(var g in orders.GroupBy(x => x.Symbol))
    {
        var symbolOrders = g.OrderBy(x => x.Created).ToList();
        foreach (var sell in symbolOrders.Where(x => x.Side == OrderSide.Sell && x.UnmatchedQuantity > 0m))
        {
            var buys = symbolOrders.Where(x => x.Side == OrderSide.Buy && x.Created < sell.Created && x.UnmatchedQuantity > 0m)
                .OrderBy(x => x.Created)
                .ToList();

            foreach (var buy in buys)
            {
                var quantityToUse = Math.Min(buy.UnmatchedQuantity.Value, sell.UnmatchedQuantity.Value);
                context.OrderRelations.Add(new OrderRelation
                {
                    BuyOrder = buy,
                    SellOrder = sell,
                    Quantity = quantityToUse,
                });

                buy.UnmatchedQuantity = buy.UnmatchedQuantity.Value - quantityToUse;
                sell.UnmatchedQuantity = sell.UnmatchedQuantity.Value - quantityToUse;
                if (sell.UnmatchedQuantity == 0m)
                {
                    break;
                }
            }
        }
    }

    context.SaveChanges();
}

async Task InsertTransferred()
{
    // "2021-02-17 15:00:41","1713811256","XRPUSDT","Market","BUY","0E-10","18.4000000000XRP","2021-02-17 15:00:41","18.4000000000XRP","0.54280000","9.9875200000USDT","FILLED"
    var xrpQty = 18.4m;
    var xrpPrice = 0.5428m;
    context.Orders.Add(new Order 
    { 
        BinanceId = 1713811256,
        Side = OrderSide.Buy,
        Symbol = "XRPUSDT",
        Status = OrderStatus.Filled,
        Quantity = xrpQty,
        ExecutedQuantity = xrpQty,
        Commission = 0.001m * xrpQty,
        RemainingQuantity = 0.999m * xrpQty,
        AverageFillPrice = xrpPrice,
        Created = new DateTimeOffset(2021, 2, 17, 15, 0, 41,TimeSpan.Zero),
        Updated = new DateTimeOffset(2021, 2, 17, 15, 0, 41, TimeSpan.Zero),
        CreateResponse = "\"2021-02-17 15:00:41\",\"1713811256\",\"XRPUSDT\",\"Market\",\"BUY\",\"0E-10\",\"18.4000000000XRP\",\"2021-02-17 15:00:41\",\"18.4000000000XRP\",\"0.54280000\",\"9.9875200000USDT\",\"FILLED\"",
        

    });
    // "2021-02-17 15:10:18","956448409","ADAUSDT","Market","BUY","0E-10","11.4000000000ADA","2021-02-17 15:10:18","11.4000000000ADA","0.87056000","9.9243840000USDT","FILLED"
    var adaQty = 11.4m;
    context.Orders.Add(new Order
    {
        BinanceId = 956448409,
        Side = OrderSide.Buy,
        Symbol = "ADAUSDT",
        Status = OrderStatus.Filled,
        Quantity = adaQty,
        ExecutedQuantity = adaQty,
        Commission = 0.001m * adaQty,
        RemainingQuantity = 0.999m * adaQty,
        AverageFillPrice = 0.87056m,
        Created = new DateTimeOffset(2021, 2, 17, 15, 10, 18, TimeSpan.Zero),
        Updated = new DateTimeOffset(2021, 2, 17, 15, 10, 18, TimeSpan.Zero),
        CreateResponse= "\"2021-02-17 15:10:18\",\"956448409\",\"ADAUSDT\",\"Market\",\"BUY\",\"0E-10\",\"11.4000000000ADA\",\"2021-02-17 15:10:18\",\"11.4000000000ADA\",\"0.87056000\",\"9.9243840000USDT\",\"FILLED\""
    });

    context.SaveChanges();
}

void RemoveDuplicatePrices()
{
    var prices = context.Prices.Where(x => x.TimeOpen >= new DateTimeOffset(2024,2,1,0,0,0,TimeSpan.Zero)).ToList();
    foreach(var group in prices.GroupBy(x => x.CryptoId))
    {
        var timestamps = new HashSet<DateTimeOffset>();
        var idsToRemove = new List<long>();
        foreach(var price in group)
        {
            if(timestamps.Contains(price.TimeOpen))
            {
                idsToRemove.Add(price.Id);
            }
            else
            {
                timestamps.Add(price.TimeOpen);
            }
        }

        foreach(var id in idsToRemove)
        {
            context.Prices.Remove(context.Prices.Find(id));
        }

        ;
    }

    context.SaveChanges();

}

//ImportCryptos();

//await FetchHourDataMonthly();
//await FetchHourDataDaily();
//ImportHourDataFromMonthlyFiles();
ImportHourDataFromDailyFiles();
//UpdateHourMovingAverages();
//await FetchMinuteData();
//ImportMinuteDataFromMonthlyFiles();
//ImportMinuteDataFromDailyFiles();
//UpdateMinuteMovingAverages();
//await CalculateStatistics();
//UpdateHourPeaks();
//UpdateHourATR();
//UpdateFeatures();
//await GenerateModels();
//await Predict();
//await UpdateCryptos();
//var sw = Stopwatch.StartNew();
//await MatchOrders();
//sw.Stop();

//RemoveDuplicatePrices();
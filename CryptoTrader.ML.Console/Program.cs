using CryptoTrader.Data;
using CryptoTrader.ML.Console;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

var services = new ServiceCollection();
services.AddDbContextFactory<BinanceContext>(x => x
    .UseNpgsql(@"")
    .UseSnakeCaseNamingConvention());

var serviceProvider = services.BuildServiceProvider();

var contextFactory = serviceProvider.GetRequiredService<IDbContextFactory<BinanceContext>>();

var context = contextFactory.CreateDbContext();



/*
CryptoTrader.ML.Console.ModelBuilder.CreateModel(1);
CryptoTrader.ML.Console.ModelBuilder.CreateModel(2);
CryptoTrader.ML.Console.ModelBuilder.CreateModel(3);
*/


// 5 13491 24/03/2023 14.00.00 +00:00 06/10/2024 16.00.00 +00:00
async Task UpdateStreaks()
{
    await Analyzer.CalculateStreaks(context);
    var streaks = await Analyzer.GetStreaks();
    Console.WriteLine("Top 50 streaks:");
    foreach (var streak in streaks.OrderByDescending(x => x.Hours).Take(50))
    {
        Console.WriteLine($"{streak.CryptoId} {streak.Hours} hours {streak.Start} - {streak.End}");
    }

}
async Task Train()
{
    var streaks = await Analyzer.GetStreaks();
    foreach (var streak in streaks.OrderByDescending(x => x.Hours).Take(100))
    {
        var cryptoId = streak.CryptoId;
        var start = streak.Start;
        var end = streak.End;

        Console.WriteLine($"{cryptoId} {start} - {end}");

        var sw1 = Stopwatch.StartNew();

        var data = await context.Prices
            .Include(x => x.Avg)
            .Include(x => x.CandleSticks)
            .Include(x => x.Cycles)
            .Include(x => x.Length)
            .Include(x => x.MA)
            .Include(x => x.Momentum)
            .Include(x => x.OtherIndicators)
            .Include(x => x.Peak)
            .Include(x => x.Proportion)
            .Include(x => x.Return)
            .Include(x => x.Trend)
            .Include(x => x.Volatility)
            .Include(x => x.Volumes)
            .Where(x => x.CryptoId == cryptoId && x.TimeOpen >= start && x.TimeOpen <= end).OrderBy(x => x.TimeOpen).ToArrayAsync();
        sw1.Stop();
        Console.WriteLine($"Data loaded: {sw1.Elapsed.TotalSeconds}");


        var sw2 = Stopwatch.StartNew();
        var models = CryptoTrader.ML.Console.ModelBuilder.MapData<PriceModel3>(data, 3);
        sw2.Stop();
        Console.WriteLine($"Data mapped: {sw2.Elapsed.TotalSeconds}");
        Console.WriteLine();
        Console.WriteLine();

        var outputs = new[]
        {
        //nameof(PriceModel3.NextOpen),
        nameof(PriceModel3.NextHigh),
        nameof(PriceModel3.NextLow),
        /*
        nameof(PriceModel3.NextClose),
        nameof(PriceModel3.NextPeakOffsetNextHH),
        nameof(PriceModel3.NextPeakOffsetNextLL),
        nameof(PriceModel3.NextReturnDayRank12),
        nameof(PriceModel3.NextReturnTwoDayRank12),
        nameof(PriceModel3.NextReturnWeekRank12),
        nameof(PriceModel3.NextReturnDayReturn),
        nameof(PriceModel3.NextReturnTwoDayReturn),
        nameof(PriceModel3.NextReturnWeekReturn)
        */
    };

        foreach (var output in outputs)
        {
            Console.WriteLine(output);
            Console.WriteLine();

            try
            {
                var sw3 = Stopwatch.StartNew();
                var stats = CryptoTrader.ML.Console.ModelBuilder.BuildModel(models, output);
                sw3.Stop();
                Console.WriteLine();
                Console.WriteLine($"Model built: {sw3.Elapsed.TotalSeconds}");

                File.AppendAllLines("model.csv",
                [
                    $"{cryptoId};{start};{end};{output};3;{stats.Trainer};{stats.Metrics.LossFunction};{stats.Metrics.RSquared};{stats.Metrics.MeanAbsoluteError};{stats.Metrics.MeanSquaredError};{stats.Metrics.RootMeanSquaredError};{stats.MaxDiff};{stats.AverageDiff}"
                ]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine();
            Console.WriteLine(new string('.', 30));
        }

        Console.WriteLine(new string('=', 30));
    }


}


await UpdateStreaks();
using CryptoTrader.BackTesting;
using CryptoTrader.Data;
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

var strategies = new Strategy[]
{
    //new MacdCrossoverStrategy(context),
    //new SmaCrossoverStrategy(context)
    new ProfitRankStrategy(context),
    new TrailingStopStrategy(context)
};

var cryptos = context.Cryptos.OrderBy(x => x.Rank).ToList();

foreach(var crypto in cryptos)
{
    var lastStreak = context.PriceStreaks
        .Where(x => x.CryptoId == crypto.Id)
        .OrderByDescending(x => x.EndTime)
        .FirstOrDefault();

    if (lastStreak == null ||lastStreak.StartTime.Year >= 2024)
    {
        continue;
    }
    foreach (var strategy in strategies)
    {
        Console.WriteLine(strategy.GetType().Name);
        Console.WriteLine(crypto.Name);
        Console.WriteLine($"{lastStreak.StartTime} - {lastStreak.EndTime}");

        var sw = Stopwatch.StartNew();
        var result = strategy.Simulate(crypto.Id, lastStreak.StartTime, lastStreak.EndTime);
        sw.Stop();

        Console.WriteLine("Profit: " + result.Profit + "%");
        Console.WriteLine("Transactions: " + result.Orders.Count());
        Console.WriteLine("Profit holding: " + result.Profit + "%");
        Console.WriteLine("Start price: " + result.StartPrice);
        Console.WriteLine("End price: " + result.EndPrice);

        Console.WriteLine();

        Console.WriteLine(sw.Elapsed.TotalSeconds + " s");
        Console.WriteLine(new string('-', 20));
    }
}
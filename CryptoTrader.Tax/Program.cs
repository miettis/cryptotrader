using CryptoTrader.Data;
using CryptoTrader.Tax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

var services = new ServiceCollection();
services.AddDbContextFactory<BinanceContext>(x => x
    .UseNpgsql(@"")
    .UseSnakeCaseNamingConvention());

var serviceProvider = services.BuildServiceProvider();
var contextFactory = serviceProvider.GetRequiredService<IDbContextFactory<BinanceContext>>();
var context = contextFactory.CreateDbContext();

var foo = new BinanceCsvUtility(context);
foo.Export();

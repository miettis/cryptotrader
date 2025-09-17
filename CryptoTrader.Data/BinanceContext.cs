using CryptoTrader.Data.Analyzers;
using CryptoTrader.Data.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

namespace CryptoTrader.Data
{
    public class BinanceContext : DbContext
    {
        public DbSet<Crypto> Cryptos { get; set; }
        public DbSet<CryptoStatistics> CryptoStatistics { get; set; }
        public DbSet<CryptoModel> CryptoModels { get; set; }


        public DbSet<Price> Prices { get; set; }
        public DbSet<PricePrediction> Predictions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderRelation> OrderRelations { get; set; }
        public DbSet<Config> Configs { get; set; }

        public DbSet<Analyzer> Analyzers { get; set; }
        public DbSet<AnalyzerOutput> AnalyzerOutputs { get; set; }
        public DbSet<Indicator> Indicators { get; set; }
        public DbSet<PriceFeature> PriceFeatures { get; set; }
        public DbSet<PriceStreak> PriceStreaks { get; set; }

        private readonly bool _useSqlServer = false;

        public BinanceContext(DbContextOptions options) : base(options)
        {
            var dbProvider = options.Extensions.First(x => x.Info.IsDatabaseProvider);
            _useSqlServer = dbProvider is SqlServerOptionsExtension;

        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Remove<ComplexTypeAttributeConvention>();
            configurationBuilder.Conventions.Add(x => new CustomComplexTypeAttributeConvention(x.GetRequiredService<ProviderConventionSetBuilderDependencies>()));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Crypto>().ToTable(_useSqlServer ? "Crypto" : "crypto");
            builder.Entity<Crypto>().HasMany(x => x.Statistics).WithOne(x => x.Crypto);
            builder.Entity<Crypto>().HasMany(x => x.Models).WithOne(x => x.Crypto);
            builder.Entity<Crypto>().HasMany(x => x.Predictions).WithOne(x => x.Crypto);
            builder.Entity<CryptoStatistics>().ToTable(_useSqlServer ? "CryptoStatistics" : "crypto_statistics");
            builder.Entity<CryptoModel>().ToTable(_useSqlServer ? "CryptoModel" : "crypto_model");     
            builder.Entity<PricePrediction>().ToTable(_useSqlServer ? "PricePrediction" : "price_prediction");
            

            builder.Entity<Price>().ToTable(_useSqlServer ? "Price" : "price");
            builder.Entity<Price>().HasOne(x => x.Crypto).WithMany(x => x.Prices);
            builder.Entity<Price>().HasOne(x => x.CandleSticks).WithOne(x => x.Price).HasForeignKey<PriceCandleSticks>();
            builder.Entity<Price>().HasOne(x => x.Cycles).WithOne(x => x.Price).HasForeignKey<PriceCycles>();
            builder.Entity<Price>().HasOne(x => x.MA).WithOne(x => x.Price).HasForeignKey<PriceMovingAverages>();
            builder.Entity<Price>().HasOne(x => x.Momentum).WithOne(x => x.Price).HasForeignKey<PriceMomentums>();
            builder.Entity<Price>().HasOne(x => x.OtherIndicators).WithOne(x => x.Price).HasForeignKey<PriceOtherIndicators>();
            builder.Entity<Price>().HasOne(x => x.Peak).WithOne(x => x.Price).HasForeignKey<PricePeak>();
            builder.Entity<Price>().HasOne(x => x.Return).WithOne(x => x.Price).HasForeignKey<PriceReturn>();
            builder.Entity<Price>().HasOne(x => x.Trend).WithOne(x => x.Price).HasForeignKey<PriceTrends>();
            builder.Entity<Price>().HasOne(x => x.Volatility).WithOne(x => x.Price).HasForeignKey<PriceVolatilities>();
            builder.Entity<Price>().HasOne(x => x.Volumes).WithOne(x => x.Price).HasForeignKey<PriceVolumes>();
            builder.Entity<Price>().HasIndex(b => new { b.CryptoId, b.TimeOpen }).IsUnique();

            builder.Entity<PriceMovingAverages>().ToTable("price_ma");

            var indicatorTypes = builder.Model.GetEntityTypes().Where(x => typeof(FeatureContainer).IsAssignableFrom(x.ClrType)).ToList();
            foreach(var type in indicatorTypes)
            {
                builder.Model.FindEntityType(type.ClrType).GetNavigations().ToList().ForEach(x =>
                {
                    if(x.ClrType == typeof(Price) || x.ClrType == typeof(Crypto))
                    {
                        return;
                    }
                    builder.Entity(type.ClrType).OwnsOne(x.ClrType, x.Name, b => 
                    { 
                        var prefix = x.PropertyInfo.GetCustomAttribute<ColumnAttribute>()?.Name ?? x.Name.ToLower();
                        foreach(var property in x.ClrType.GetProperties())
                        {
                            var columnName = property.GetCustomAttribute<ColumnAttribute>()?.Name ?? property.Name.ToLower();
                            
                            b.Property(property.PropertyType, property.Name).HasColumnName($"{prefix}_{columnName}");
                            
                        }
                    });
                });
            }

            

            builder.Entity<Order>().ToTable(_useSqlServer ? "Order" : "order");
            builder.Entity<Order>().HasMany(x => x.SellOrders).WithOne(x => x.BuyOrder).HasForeignKey(x => x.BuyOrderId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>().HasMany(x => x.BuyOrders).WithOne(x => x.SellOrder).HasForeignKey(x => x.SellOrderId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>().Property(e => e.Side).HasConversion<string>();

            builder.Entity<OrderRelation>().ToTable(_useSqlServer ? "OrderRelation" : "order_relation");

            builder.Entity<Config>().ToTable(_useSqlServer ? "Config" : "config");

            

            foreach (var property in builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()).Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                if(property.PropertyInfo.GetCustomAttribute<PrecisionAttribute>() == null)
                {
                    property.SetPrecision(35);
                    property.SetScale(15);
                }
            }

            var futureTypes = new[] { typeof(PricePeak), typeof(PriceReturn) };
            foreach(var property in builder.Model.GetEntityTypes().Where(x => futureTypes.Contains(x.ClrType)).SelectMany(t => t.GetProperties()).Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                var comment = property.GetComment() ?? "";
                comment += "\r\nfuture";
                property.SetComment(comment.Trim());
            }


            builder.Entity<Price>()
                .HasMany(x => x.Features)
                .WithOne(x => x.Price)
                .HasForeignKey(x => x.PriceId);

            builder.Entity<PriceFeature>().ToTable("price_feature");

            builder.Entity<PriceStreak>().ToTable("price_streak");
        }

        /*
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<decimal>().HavePrecision(18, 6);
            configurationBuilder.Properties<decimal?>().HavePrecision(18, 6);
        }
        */

        public async Task UpdateCryptoDataEndTime() 
        {

            if (_useSqlServer)
            {
                await Database.ExecuteSqlRawAsync("UPDATE Crypto SET Times_EndData = (SELECT MAX(TimeOpen) FROM PriceHour WHERE CryptoId = Crypto.Id)");
            }
            else
            {
                await Database.ExecuteSqlRawAsync("UPDATE crypto SET end_data = (SELECT MAX(time_open) FROM price WHERE crypto_id = crypto.id)");
            }
        }
        public async Task UpdateCryptoDataEndTime(int cryptoId)
        {
            if (_useSqlServer)
            {
                await Database.ExecuteSqlRawAsync($"UPDATE Crypto SET Times_EndData = (SELECT MAX(TimeOpen) FROM PriceHour WHERE CryptoId = {cryptoId})");
            }
            else
            {
                await Database.ExecuteSqlRawAsync($"UPDATE crypto SET end_data = (SELECT MAX(time_open) FROM price WHERE crypto_id = {cryptoId})");
            }
        }

        public async Task<CryptoStatistics> GetCryptoStatisticsAsync(int? cryptoId, DateTimeOffset start, DateTimeOffset end)
        {
            if (_useSqlServer)
            {
                throw new NotImplementedException();
            }
            var percentiles = new[]
            {
                "0.05",
                "0.10",
                "0.25",
                "0.50",
                "0.75",
                "0.90",
                "0.95"
            };
            var groups = new[]
            {
                ("candle","length"),
                ("body","length"),
                ("upper","length"),
                ("lower","length"),
                //("candle","proportion"),
                ("body","proportion"),
                ("upper","proportion"),
                ("lower","proportion")
            };
            var sql = "";
            foreach (var group in groups)
            {
                sql += $",AVG({group.Item2}_{group.Item1}) as {group.Item1}_{group.Item2}_mean,TRUNC(STDDEV_POP({group.Item2}_{group.Item1}), 28) as {group.Item1}_{group.Item2}_std";
                foreach (var percentile in percentiles)
                {
                    var percentileNum = (int)(decimal.Parse(percentile, CultureInfo.InvariantCulture) * 100);
                    sql += $",percentile_disc({percentile}) within group (order by {group.Item2}_{group.Item1}) AS {group.Item1}_{group.Item2}_lim{percentileNum}";
                }
            }
            if (cryptoId.HasValue)
            {
                sql = "SELECT " + sql.Substring(1) + $" FROM price WHERE crypto_id={cryptoId} AND time_open >= '{start:yyyy-MM-dd HH':'mm':'ss}' AND time_open <= '{end:yyyy-MM-dd HH':'mm':'ss}'";
            }
            else
            {
                sql = "SELECT " + sql.Substring(1) + $" FROM price WHERE time_open >= '{start:yyyy-MM-dd HH':'mm':'ss}' AND time_open <= '{end:yyyy-MM-dd HH':'mm':'ss}'";
            }
            var conn = Database.GetDbConnection();
            using var cmd = conn.CreateCommand();
            if (conn.State != ConnectionState.Open) 
            { 
                await conn.OpenAsync();
            }
            cmd.CommandText = sql;

            var statistics = new CryptoStatistics();
            statistics.CryptoId = cryptoId ?? Data.CryptoStatistics.StatsCryptoId;
            statistics.StartTime = start;
            statistics.EndTime = end;

            using var reader = await cmd.ExecuteReaderAsync();
            if(reader.Read())
            {
                foreach(var group in groups)
                {
                    var partProperty = statistics.GetType().GetProperties().First(x => x.Name.Equals($"{group.Item1}", StringComparison.OrdinalIgnoreCase));
                    var part = partProperty.GetValue(statistics) as CandlePartStatistics;

                    var attrProperty = partProperty.PropertyType.GetProperties().First(x => x.Name.Equals($"{group.Item2}", StringComparison.OrdinalIgnoreCase));
                    var limits = attrProperty.GetValue(part) as CandleLimits;

                    limits.Mean = reader.GetDecimal(reader.GetOrdinal($"{group.Item1}_{group.Item2}_mean"));
                    limits.Std = reader.GetDecimal(reader.GetOrdinal($"{group.Item1}_{group.Item2}_std"));

                    foreach (var percentile in percentiles)
                    {
                        var percentileNum = (int)(decimal.Parse(percentile, CultureInfo.InvariantCulture) * 100);
                        var value = reader.GetDecimal(reader.GetOrdinal($"{group.Item1}_{group.Item2}_lim{percentileNum}"));

                        var limProperty = attrProperty.PropertyType.GetProperties().First(x => x.Name.Equals($"Lim{percentileNum}", StringComparison.OrdinalIgnoreCase));
                        limProperty.SetValue(limits, value);
                    }
                }
                
            }
            return statistics;
        }
        public async Task GetBuyOrders(Order sellOrder)
        {
            var previousSellOrder = await Orders.Where(x => x.Symbol == sellOrder.Symbol && x.Side == OrderSide.Sell && x.Created < sellOrder.Created).OrderByDescending(x => x.Created).FirstOrDefaultAsync();
            var buyOrders = await Orders.Where(x => x.Symbol == sellOrder.Symbol && x.Side == OrderSide.Buy && x.Created > previousSellOrder.Created && x.Created < sellOrder.Created).ToListAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CryptoTrader.Data
{
    public class BinanceContextFactory : IDesignTimeDbContextFactory<BinanceContext>
    {
        public BinanceContext CreateDbContext(string[] args)
        {
            return CreateDbContextPostgreSQL(false);
        }

        public static BinanceContext CreateDbContextSqlServer()
        {
            var optionsBuilder = new DbContextOptionsBuilder<BinanceContext>();
            optionsBuilder.UseSqlServer(@"");

            return new BinanceContext(optionsBuilder.Options);
        }
        public static BinanceContext CreateDbContextPostgreSQL(bool local = true)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BinanceContext>();
            optionsBuilder.UseNpgsql(@$"")
                .UseSnakeCaseNamingConvention();

            return new BinanceContext(optionsBuilder.Options);
        }
    }
}

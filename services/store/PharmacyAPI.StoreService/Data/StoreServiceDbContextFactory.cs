using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PharmacyAPI.StoreService.Data;

/* EF Core komut satiri araclari (dotnet ef migrations add / database update) icin gerekli.
 * Proje klasorunden calistirildigi varsayilir; appsettings.json oradan okunur. */
public class StoreServiceDbContextFactory : IDesignTimeDbContextFactory<StoreServiceDbContext>
{
    public StoreServiceDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var builder = new DbContextOptionsBuilder<StoreServiceDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new StoreServiceDbContext(builder.Options);
    }
}

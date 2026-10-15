using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PharmacyAPI.CustomerService.Data;

/* EF Core komut satiri araclari (dotnet ef migrations add / database update) icin gerekli.
 * Proje klasorunden calistirildigi varsayilir; appsettings.json oradan okunur. */
public class CustomerServiceDbContextFactory : IDesignTimeDbContextFactory<CustomerServiceDbContext>
{
    public CustomerServiceDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var builder = new DbContextOptionsBuilder<CustomerServiceDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new CustomerServiceDbContext(builder.Options);
    }
}

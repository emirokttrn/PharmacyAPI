using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PharmacyAPI.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class PharmacyAPIDbContextFactory : IDesignTimeDbContextFactory<PharmacyAPIDbContext>
{
    public PharmacyAPIDbContext CreateDbContext(string[] args)
    {
        PharmacyAPIEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<PharmacyAPIDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new PharmacyAPIDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../PharmacyAPI.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}

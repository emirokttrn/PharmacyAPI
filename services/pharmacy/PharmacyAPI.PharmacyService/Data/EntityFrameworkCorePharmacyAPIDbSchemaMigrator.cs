using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PharmacyAPI.Data;
using Volo.Abp.DependencyInjection;

namespace PharmacyAPI.EntityFrameworkCore;

public class EntityFrameworkCorePharmacyAPIDbSchemaMigrator
    : IPharmacyAPIDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCorePharmacyAPIDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the PharmacyAPIDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<PharmacyAPIDbContext>()
            .Database
            .MigrateAsync();
    }
}

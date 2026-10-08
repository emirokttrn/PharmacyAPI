using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace PharmacyAPI.Data;

/* This is used if database provider does't define
 * IPharmacyAPIDbSchemaMigrator implementation.
 */
public class NullPharmacyAPIDbSchemaMigrator : IPharmacyAPIDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}

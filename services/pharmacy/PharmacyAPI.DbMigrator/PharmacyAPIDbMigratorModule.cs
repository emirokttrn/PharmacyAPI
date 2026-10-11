using PharmacyAPI.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace PharmacyAPI.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(PharmacyAPIEntityFrameworkCoreModule),
    typeof(PharmacyAPIApplicationContractsModule)
    )]
public class PharmacyAPIDbMigratorModule : AbpModule
{
}

using System;
using Microsoft.Extensions.DependencyInjection;
using PharmacyAPI.Categories;
using PharmacyAPI.IRepositories;
using Volo.Abp.Uow;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace PharmacyAPI.EntityFrameworkCore;

[DependsOn(
    typeof(PharmacyAPIDomainModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule)
    )]
public class PharmacyAPIEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PharmacyAPIEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<PharmacyAPIDbContext>(options =>
        {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        Configure<AbpDbContextOptions>(options =>
        {
                /* The main point to change your DBMS.
                 * See also PharmacyAPIMigrationsDbContextFactory for EF Core tooling. */
            options.UseSqlServer();
        });

        // [Claude Agent] - ABP'nin conventional repository auto-registration'i (nedeni belirsiz,
        // digerleri -Brand, Product, Order- ayni pattern'le calisiyor) ICategoryRepository'yi
        // otomatik kaydetmiyordu; CategoryManger ve ProductManager constructor'larinda
        // "Cannot resolve parameter ICategoryRepository" hatasiyla TUM category ve product
        // endpoint'lerini kirip 500 donduruyordu. Elle (explicit) kayit ile bypass ediyoruz.
        context.Services.AddTransient<ICategoryRepository, CategroyRepository>();
    }
}

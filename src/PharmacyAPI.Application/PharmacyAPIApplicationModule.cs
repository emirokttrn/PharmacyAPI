using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Microsoft.Extensions.DependencyInjection;
using PharmacyAPI.CategroyServices;
using PharmacyAPI.IServices.Category;

namespace PharmacyAPI;

[DependsOn(
    typeof(PharmacyAPIDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(PharmacyAPIApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
public class PharmacyAPIApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<PharmacyAPIApplicationModule>();
        });

        // [Claude Agent] - ABP'nin conventional ApplicationService auto-registration'i (nedeni
        // belirsiz) IcategoryService'i otomatik kaydetmiyordu; CategroyController'in constructor'i
        // "Cannot resolve parameter IcategoryService service" hatasiyla TUM category endpoint'lerini
        // kirip 500 donduruyordu (bkz. ICategoryRepository icin PharmacyAPIEntityFrameworkCoreModule'daki
        // ayni sebepli fix). Elle (explicit) kayit ile bypass ediyoruz.
        context.Services.AddTransient<IcategoryService, CategroyService>();
    }
}
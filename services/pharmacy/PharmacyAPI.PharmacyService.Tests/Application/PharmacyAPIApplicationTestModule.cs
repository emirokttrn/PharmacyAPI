using Volo.Abp.Modularity;

namespace PharmacyAPI;

[DependsOn(
    typeof(PharmacyAPIApplicationModule),
    typeof(PharmacyAPIDomainTestModule)
)]
public class PharmacyAPIApplicationTestModule : AbpModule
{

}

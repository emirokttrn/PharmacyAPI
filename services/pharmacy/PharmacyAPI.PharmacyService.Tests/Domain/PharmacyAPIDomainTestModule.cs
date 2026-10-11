using Volo.Abp.Modularity;

namespace PharmacyAPI;

[DependsOn(
    typeof(PharmacyAPIDomainModule),
    typeof(PharmacyAPITestBaseModule)
)]
public class PharmacyAPIDomainTestModule : AbpModule
{

}

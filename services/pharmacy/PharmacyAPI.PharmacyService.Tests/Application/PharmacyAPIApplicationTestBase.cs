using Volo.Abp.Modularity;

namespace PharmacyAPI;

public abstract class PharmacyAPIApplicationTestBase<TStartupModule> : PharmacyAPITestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}

using Volo.Abp.Modularity;

namespace PharmacyAPI;

/* Inherit from this class for your domain layer tests. */
public abstract class PharmacyAPIDomainTestBase<TStartupModule> : PharmacyAPITestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}

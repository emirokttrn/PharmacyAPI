using PharmacyAPI.Samples;
using Xunit;

namespace PharmacyAPI.EntityFrameworkCore.Domains;

[Collection(PharmacyAPITestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<PharmacyAPIEntityFrameworkCoreTestModule>
{

}

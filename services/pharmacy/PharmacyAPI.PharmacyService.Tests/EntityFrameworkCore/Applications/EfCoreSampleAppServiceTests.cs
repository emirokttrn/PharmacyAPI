using PharmacyAPI.Samples;
using Xunit;

namespace PharmacyAPI.EntityFrameworkCore.Applications;

[Collection(PharmacyAPITestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<PharmacyAPIEntityFrameworkCoreTestModule>
{

}

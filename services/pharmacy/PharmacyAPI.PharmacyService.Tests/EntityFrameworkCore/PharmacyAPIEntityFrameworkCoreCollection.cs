using Xunit;

namespace PharmacyAPI.EntityFrameworkCore;

[CollectionDefinition(PharmacyAPITestConsts.CollectionDefinitionName)]
public class PharmacyAPIEntityFrameworkCoreCollection : ICollectionFixture<PharmacyAPIEntityFrameworkCoreFixture>
{

}

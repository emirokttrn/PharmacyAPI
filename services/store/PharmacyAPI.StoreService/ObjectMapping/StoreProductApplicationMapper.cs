using PharmacyAPI.StoreService.Entities.StoreProducts;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace PharmacyAPI.StoreService.ObjectMapping;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class StoreProductApplicationMapper : MapperBase<StoreProduct, StoreProductDto>
{
    public override partial StoreProductDto Map(StoreProduct source);
    public override partial void Map(StoreProduct source, StoreProductDto destination);
}

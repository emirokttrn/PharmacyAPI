using PharmacyAPI.Brands;
using PharmacyAPI.BranDtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace PharmacyAPI.BrandServices
{
    [Mapper]
    public partial class BrandApplicationMapper : MapperBase<Brand, BrandDto>
    {
        public override partial BrandDto Map(Brand source);
        public override partial void Map(Brand source, BrandDto destination);
    }
}
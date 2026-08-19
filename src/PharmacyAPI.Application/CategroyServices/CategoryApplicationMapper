// PharmacyAPI.Application/CategoryServices/CategoryApplicationMapper.cs
using PharmacyAPI.Categories;
using PharmacyAPI.CategoryDtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace PharmacyAPI.CategoryServices
{
    [Mapper]
    public partial class CategoryApplicationMapper : MapperBase<Category, CategoryDto>
    {
        public override partial CategoryDto Map(Category source);
        public override partial void Map(Category source, CategoryDto destination);
    }
}
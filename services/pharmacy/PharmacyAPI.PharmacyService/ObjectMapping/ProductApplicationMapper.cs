// PharmacyAPI.Application/ProductsService/ProductApplicationMapper.cs
using PharmacyAPI.Products;
using PharmacyAPI.ProductDtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace PharmacyAPI.ProductsServices
{
    [Mapper]
    public partial class ProductApplicationMapper : MapperBase<Product, ProductDto>
    {
        public override partial ProductDto Map(Product source);
        public override partial void Map(Product source, ProductDto destination);
    }
}
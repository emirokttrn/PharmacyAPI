using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.ProductDtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PharmacyAPI.IServices.Product
{
     public interface IProductService : IApplicationService
     {
          Task<ProductDto> GetAsync(Guid id);

          Task<PagedResultDto<ProductDto>> GetlistAsync(ProductListFilterDto request);
          Task<ProductDto> CreateAsync(CreateProductDto request);

          Task<ProductDto> UpdateAsync(Guid id, CreateProductDto request);

          Task DeleteAsync(Guid id);

          Task<List<ProductDto>> GetBestSellerProductsAsync(int count);

          Task<List<ProductDto>> GetNewProductsAsync(int count);
          Task<List<ProductDto>> GetDiscountedProductsAsync(int count);

          Task<PagedResultDto<ProductDto>> GetListByCategoryAsync(Guid id, ProductListFilterDto request);

          Task DeleteManyAsync(List<Guid> ids);

          Task SetStockStatusAsync(Guid id, bool inStock);
          Task ChangeManyStockStatus(List<Guid> id, bool inStoock);
     }

}
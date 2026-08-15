using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.BranDtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PharmacyAPI.IServices.Brand
{
    public interface IBrandService :IApplicationService
    {
        Task<BrandDto> GetAsync(Guid id);
         Task<PagedResultDto<BrandDto>> GetListAsync(FilterBrandDto request);
        Task<BrandDto> CreateAsync(CreateBrandDto request);
        Task<BrandDto> UpdateAsync(Guid id, CreateBrandDto request);
        Task DeleteAsync(Guid id);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.CategoryDtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PharmacyAPI.IServices.Category
{
    public interface IcategoryService : IApplicationService
    {
        Task<CategoryDto> GetAsync(Guid id);

        Task<List<CategoryDto>> GetListAsync();

        Task<PagedResultDto<CategoryDto>> FilterCategoryListAsync(CategoryFilterDto request);

        Task<CategoryDto> CreateCategory(CreateCategoryDto request);

        Task<CategoryDto> UpdateCategroy(Guid id, CreateCategoryDto request);

        Task DeleteAsync(Guid id);
    }
}
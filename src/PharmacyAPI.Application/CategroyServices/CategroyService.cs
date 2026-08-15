using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.Categories;
using PharmacyAPI.CategoryDtos;
using PharmacyAPI.DomainServices;
using PharmacyAPI.IRepositories;
using PharmacyAPI.IServices.Category;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace PharmacyAPI.CategroyServices
{
    public class CategroyService : ApplicationService, IcategoryService
    {
           private readonly ICategoryRepository _repository;
            private readonly CategoryManger _categoryManager; 


        public CategroyService(ICategoryRepository repository, CategoryManger categoryManager)
        {
            _repository = repository;
            _categoryManager = categoryManager;

        }
        public async Task<CategoryDto> CreateCategory(CreateCategoryDto request)
        {
           var createCategroy =  await _categoryManager.CreateAsync(request.CategorName, request.guid);
           await _repository.InsertAsync(createCategroy);
           
           return ObjectMapper.Map<Category, CategoryDto>(createCategroy);
        }

        public async Task DeleteAsync(Guid id)
        {
        var findId=   await _repository.FindAsync(id);
        if(findId==null)
        {throw new UserFriendlyException("id bulunamadi");}
    await _repository.DeleteAsync(id);
        }

        public async Task<PagedResultDto<CategoryDto>> FilterCategoryListAsync(CategoryFilterDto request)
        {
            if(request.Sorting.IsNullOrWhiteSpace())
            {
                request.Sorting= nameof(Category.CategoryName);

            }
            var result = await _repository.GetPagedListAsync(request.SkipCount,request.MaxResultCount, request.Sorting);
            var totalCount = await _repository.GetCountAsync();
              return new PagedResultDto<CategoryDto>(totalCount, ObjectMapper.Map<List<Category>, List<CategoryDto>>(result));
        }

        public async Task<CategoryDto> GetAsync(Guid id)
        {
            var findId= await _repository.FirstOrDefaultAsync(c=>c.Id==id);
            if(findId==null) throw new UserFriendlyException("boyle bi id yok");
            return ObjectMapper.Map<Category, CategoryDto>(findId);
        }

        public async Task<List<CategoryDto>> GetListAsync()
        {
          var result=  await _repository.GetListAsync();
          return ObjectMapper.Map<List<Category>, List<CategoryDto>>(result);
        }

        public async Task<CategoryDto> UpdateCategroy(Guid id, CreateCategoryDto request)
        {
            var category= await _repository.GetAsync(id);
        

            if(category==null){throw new UserFriendlyException("boyle bi id yok ");}
            category.CategoryName= request.CategorName;
            category.ParentId=request.guid;
            await _repository.UpdateAsync(category);
            return ObjectMapper.Map<Category, CategoryDto>(category);

        }
    }
}
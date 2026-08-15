using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.Categories;
using PharmacyAPI.IRepositories;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace PharmacyAPI.DomainServices
{
    public class CategoryManger : DomainService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IGuidGenerator _guidGenerator;
        public CategoryManger(ICategoryRepository categoryRepository,IGuidGenerator guidGenerator)
        {
            _categoryRepository=categoryRepository;
            _guidGenerator=guidGenerator;
        }

        public async Task<Category> CreateAsync(string categoryName, Guid? parentId=null)
        {
            var existingCategory = await _categoryRepository.FirstOrDefaultAsync(c=>c.CategoryName==categoryName);
            if(existingCategory!=null)
            {
                throw new UserFriendlyException($"{categoryName} adinda bir kategori zaten var.");
            }
            if(parentId.HasValue)
            {
                await _categoryRepository.GetAsync(parentId.Value);
            }
            return new Category(_guidGenerator.Create(), categoryName, parentId);
        }
    }
}
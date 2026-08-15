using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.Categories;
using PharmacyAPI.IRepositories;
using PharmacyAPI.Products;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace PharmacyAPI.DomainServices
{
    public class ProductManager : DomainService
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IGuidGenerator guidGenerator;
        public ProductManager(IProductRepository productRepository,IBrandRepository brandRepository, ICategoryRepository categoryRepository)
        {
            _productRepository=productRepository;
            _brandRepository=brandRepository;
            _categoryRepository=categoryRepository;
        }
public async Task<Product> CreateAsync(string productName, Guid brandId, Guid categoryId, decimal price)
        {
            var existingBrand= await _brandRepository.FirstOrDefaultAsync(b=>b.Id==brandId);
          if(existingBrand==null) {throw new UserFriendlyException("boyle bi marka yok once markayi olusturun! ");}
          var existingCategory= await _categoryRepository.FirstOrDefaultAsync(c=>c.Id==categoryId);
          if(existingCategory==null){throw new UserFriendlyException("dogru kategoriyi sectiginizden emin olun!");}
          return new Product(
            guidGenerator.Create(),
            productName,
            brandId,
            categoryId,
            price
          );
          
        }




        // public async Task<Category> CreateCategoryAsync(string categroyName, string ProductName)
        // {
        //     var existingCategory = await _categoryRepository.FirstOrDefaultAsync(c=>c.CategoryName==categroyName);
        //     if(existingCategory!=null)
        //     throw new UserFriendlyException($"{categroyName} boyle bi kategori var");
        //     return new Category()
        // }
    }
}
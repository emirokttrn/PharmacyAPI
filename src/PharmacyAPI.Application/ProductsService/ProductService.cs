using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using PharmacyAPI.ProductDtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using PharmacyAPI.Products;
using Volo.Abp;
using PharmacyAPI.HealtTopics;
using System.Linq.Dynamic.Core;
using Volo.Abp.Guids;
using PharmacyAPI.IServices.Product;
using PharmacyAPI.IRepositories;
using PharmacyAPI.DomainServices;

namespace PharmacyAPI.ProductsService
{
    public class ProductService : ApplicationService, IProductAppService
    {

        private readonly IProductRepository _repository;
        private readonly ProductManager _productManager;


        public ProductService(IProductRepository repository,ProductManager productManager)
        {
            _repository = repository;
           _productManager= productManager;

        }
        public Task ChangeManyStockStatus(List<Guid> id, bool inStoock)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto request)
        {
            var newProduct = await _productManager.CreateAsync(request.ProductName, request.BrandId, request.CategoryId,request.Price);

            newProduct.SetDiscountedPrice(request.DiscountedPrice);
            newProduct.SetCountry(request.Country);
            newProduct.SetImage(request.Image);
            newProduct.SetActiveIngredient(request.ActiveIngredient);
            newProduct.SetDescription(request.Description);
            newProduct.SetBadge(request.Badge);
            newProduct.SetBadgeLabel(request.BadgeLabel);
            newProduct.SetGender(request.Gender);
            newProduct.SetAgeRange(request.AgeRange);
            newProduct.SetProductForm(request.ProductForm);
            newProduct.SetWeight(request.Weight);
            newProduct.SetStock(request.InStock);
            foreach (var topic in request.HealthToPICS)
            {
                newProduct.healthTopics.Add(new ProductHealthTopic(newProduct.Id, topic));
            }

            await _repository.InsertAsync(newProduct);

            return ObjectMapper.Map<Product, ProductDto>(newProduct);

        }

        public async Task DeleteAsync(Guid id)
        {
            var findId = await _repository.FindAsync(id);
            if (id == null)
            {
                throw new UserFriendlyException("boyle bi id'ye sahip kullanici yok");
            }
            await _repository.DeleteAsync(id);
        }

        public async Task DeleteManyAsync(List<Guid> ids)
        {
            await _repository.DeleteManyAsync(ids);
        }

        public async Task<ProductDto> GetAsync(Guid id)
        {
            var product = await _repository.GetAsync(id);
            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        public async Task<List<ProductDto>> GetBestSellerProductsAsync(int count)
        {
            if (count < 0)
            {
                throw new UserFriendlyException("0 ve asagisi olmaz");
            }
            var queryable = await _repository.GetQueryableAsync();
            var result = queryable.OrderByDescending(bs => bs.Rating).
            Take(count).
            ToList();// burda queryable kullanarak siralama yapiyoeuaz raitinge gore ve girilen count sayisi miktarinda getirecek
            return ObjectMapper.Map<List<Product>, List<ProductDto>>(result);
        }

        public async Task<List<ProductDto>> GetDiscountedProductsAsync(int count)
{
    if (count <= 0)
    {
        throw new UserFriendlyException("count 0 ve asagisi olamaz");
    }

    var queryable = await _repository.GetQueryableAsync();

    var result = queryable
        .Where(p => p.DiscountedPrice != null)
        .OrderByDescending(p => (p.Price - p.DiscountedPrice) / p.Price)
        .Take(count)
        .ToList();

    return ObjectMapper.Map<List<Product>, List<ProductDto>>(result);
}

        public async Task<PagedResultDto<ProductDto>> GetlistAsync(ProductListFilterDto request)
        {
            if (request.Sorting.IsNullOrWhiteSpace())
            {
                request.Sorting = nameof(Product.ProductName);
            }
            var result = await _repository.GetPagedListAsync(request.SkipCount, request.MaxResultCount, request.Sorting);
            var totalCount = await _repository.GetCountAsync();
            return new PagedResultDto<ProductDto>(totalCount, ObjectMapper.Map<List<Product>, List<ProductDto>>(result));
        }

        public async Task<PagedResultDto<ProductDto>> GetListByCategoryAsync(Guid id, ProductListFilterDto request)
        {
            // ben yazmadim bunu mantigini tam cozemedim cunku 
            if (request.Sorting.IsNullOrWhiteSpace())
            {
                request.Sorting = nameof(Product.ProductName);
            }
            var queryable = await _repository.GetQueryableAsync();
            queryable = queryable.Where(p => p.CategoryId == id);

            var totalCount = queryable.Count();

            var products = queryable
                .OrderBy(request.Sorting)
                .Skip(request.SkipCount)
                .Take(request.MaxResultCount)
                .ToList();

                  var productDtos = ObjectMapper.Map<List<Product>, List<ProductDto>>(products);

    return new PagedResultDto<ProductDto>(totalCount, productDtos); 
        }

        public async Task<List<ProductDto>> GetNewProductsAsync(int count)
        {
            if (count < 0)
            {

                throw new UserFriendlyException("0 ve asagisi olmaz");
            }
            //lp = last products oluyor
            var queryable = await _repository.GetQueryableAsync();
            var result = queryable.OrderByDescending(lp => lp.CreationTime).Take(count).ToList();
            return ObjectMapper.Map<List<Product>, List<ProductDto>>(result);
        }

      public async Task SetStockStatusAsync(Guid id, bool inStock)
{
    var product = await _repository.GetAsync(id);
    product.SetStock(inStock);
    await _repository.UpdateAsync(product);
}

   public async Task<ProductDto> UpdateAsync(Guid id, CreateProductDto request)
{
    var product = await _repository.GetAsync(id);

    product.SetName(request.ProductName);
    product.SetBrand(request.BrandId);
    product.SetCategory(request.CategoryId);
    product.SetPrice(request.Price);
    product.SetDiscountedPrice(request.DiscountedPrice);
    product.SetCountry(request.Country);
    product.SetImage(request.Image);
    product.SetActiveIngredient(request.ActiveIngredient);
    product.SetDescription(request.Description);
    product.SetBadge(request.Badge);
    product.SetBadgeLabel(request.BadgeLabel);
    product.SetGender(request.Gender);
    product.SetAgeRange(request.AgeRange);
    product.SetProductForm(request.ProductForm);
    product.SetWeight(request.Weight);
    product.SetStock(request.InStock);

    product.healthTopics.Clear();
    foreach (var topic in request.HealthToPICS)
    {
        product.healthTopics.Add(new ProductHealthTopic(product.Id, topic));
    }

    await _repository.UpdateAsync(product);

   return ObjectMapper.Map<Product, ProductDto>(product);

    }
}
}
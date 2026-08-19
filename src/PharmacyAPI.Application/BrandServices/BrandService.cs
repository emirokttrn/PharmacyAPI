using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.Brands;
using PharmacyAPI.BranDtos;
using PharmacyAPI.DomainServices;
using PharmacyAPI.IRepositories;
using PharmacyAPI.IServices.Brand;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace PharmacyAPI.BrandServices
{
    public class BrandService : ApplicationService, IBrandService
    {
        private readonly IBrandRepository _repository;
         private readonly BrandManager _brandManager;
        


        public BrandService(IBrandRepository repository,BrandManager brandManager)
        {
            _repository = repository;
            _brandManager=brandManager;

        }
        public async Task<BrandDto> CreateAsync(CreateBrandDto request)
        {
            var newBrand = await _brandManager.CreateAsync(request.BrandName);// brand domain service'te kontrol yapiyor
            //is mantiginin bi kismi orda
            await _repository.InsertAsync(newBrand);
            return ObjectMapper.Map<Brand, BrandDto>(newBrand);
        }

        public async Task DeleteAsync(Guid id)
        {
            var brand = await _repository.FindAsync(id);
            if (brand == null)
            {
                throw new UserFriendlyException("id bulunamadi");
            }
            await _repository.DeleteAsync(id);
        }

        public async Task<BrandDto> GetAsync(Guid id)
        {
            var brand = await _repository.GetAsync(id);
            return ObjectMapper.Map<Brand, BrandDto>(brand);
        }

        public async Task<PagedResultDto<BrandDto>> GetListAsync(FilterBrandDto request)
        {
            if (request.Sorting.IsNullOrWhiteSpace())
            {
                request.Sorting = nameof(Brand.BrandName);
            }
            var result = await _repository.GetPagedListAsync(request.SkipCount, request.MaxResultCount, request.Sorting);
            var totalCount = await _repository.GetCountAsync();
            return new PagedResultDto<BrandDto>(totalCount, ObjectMapper.Map<List<Brand>, List<BrandDto>>(result));
        }

        public async Task<BrandDto> UpdateAsync(Guid id, CreateBrandDto request)
        {
            var brand = await _repository.GetAsync(id);

            brand.SetName(request.BrandName);

            await _repository.UpdateAsync(brand);

            return ObjectMapper.Map<Brand, BrandDto>(brand);
        }
    }
}
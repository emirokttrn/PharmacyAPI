using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PharmacyAPI.Brands;
using PharmacyAPI.IRepositories;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace PharmacyAPI.DomainServices
{
    public class BrandManager : DomainService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IGuidGenerator _guidGenerator;
        public BrandManager(IBrandRepository brandRepository, IGuidGenerator guidGenerator)
        {
            _brandRepository = brandRepository;
            _guidGenerator = guidGenerator;
        }


        public async Task<Brand> CreateAsync(string name)// domain service de rerturn type her zaman entity olmali ! unutma
        {
            var existingBrand = await _brandRepository
        .FirstOrDefaultAsync(b => b.BrandName == name);

            if (existingBrand != null)
            {
                throw new UserFriendlyException($"{name} adinda bir marka zaten var.");
            }

            return new Brand(_guidGenerator.Create(), name);
        }

        public async Task<Brand> UpdateAsync(Guid id, string name)
        {
            var brand = await _brandRepository.GetAsync(id);

            var changeName = await _brandRepository.AnyAsync(b => b.BrandName == name && b.Id != id);

            if (changeName)
            {
                throw new UserFriendlyException($"{name} adinda baska bi marka var zaten");
            }
            brand.SetName(name);
            return brand;

        }
        public async Task DeleteAsync( string name)
        {
            var deleteByName= await _brandRepository.FindByNameAsync(name);

            if(deleteByName==null)
            {
                throw new UserFriendlyException($"{name} boyle isimde marka yok");
            }
            await _brandRepository.DeleteAsync(deleteByName);

        }


    }



}
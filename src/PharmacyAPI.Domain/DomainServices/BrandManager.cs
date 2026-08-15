using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.Brands;
using PharmacyAPI.IRepositories;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace PharmacyAPI.DomainServices
{
    public class BrandManager :DomainService
    {
         private readonly IBrandRepository _brandRepository;
        private readonly IGuidGenerator _guidGenerator;
        public BrandManager(IBrandRepository brandRepository,IGuidGenerator guidGenerator)
        {
            _brandRepository=brandRepository;
            _guidGenerator=guidGenerator;
        }


        public async Task<Brand> CreateAsync(string name)
        {
            var existingBrand = await _brandRepository
        .FirstOrDefaultAsync(b => b.BrandName == name);

    if (existingBrand != null)
    {
        throw new UserFriendlyException($"{name} adinda bir marka zaten var.");
    }

    return new Brand(_guidGenerator.Create(), name);
        }
    }
}
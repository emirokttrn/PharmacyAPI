using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmacyAPI.EntityFrameworkCore;
using PharmacyAPI.IRepositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PharmacyAPI.Brands
{
    public class BrandRepository : EfCoreRepository<PharmacyAPIDbContext, Brand, Guid>, IBrandRepository
    {

        public BrandRepository(IDbContextProvider<PharmacyAPIDbContext> DbContextProvider) : base(DbContextProvider)
        {

        }
        public virtual async Task<Brand?> FindByNameAsync(string name)
        {
            var dbSet = await GetDbSetAsync(); //abp'nin methodu bu dbcontexten aliyor bunu
            return await dbSet.FirstOrDefaultAsync(b => b.BrandName == name);
        }

    }

}
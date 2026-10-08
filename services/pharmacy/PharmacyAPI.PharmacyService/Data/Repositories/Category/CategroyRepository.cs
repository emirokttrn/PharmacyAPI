using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmacyAPI.EntityFrameworkCore;
using PharmacyAPI.IRepositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PharmacyAPI.Categories
{
    public class CategroyRepository : EfCoreRepository<PharmacyAPIDbContext,Category,Guid>, ICategoryRepository
    {
        public CategroyRepository(IDbContextProvider<PharmacyAPIDbContext> dbContextProvider): base(dbContextProvider){}


        public async Task<Category?> FindByNameAsync(string name)
        {
            var dbSet= await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(c=>c.CategoryName==name);
        }
    }
}
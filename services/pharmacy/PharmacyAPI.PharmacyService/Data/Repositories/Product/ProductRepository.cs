using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PharmacyAPI.EntityFrameworkCore;
using PharmacyAPI.IRepositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using System;

namespace PharmacyAPI.Products
{
     public class ProductRepository : EfCoreRepository<PharmacyAPIDbContext,Product,Guid>,IProductRepository
    {
        public ProductRepository(IDbContextProvider<PharmacyAPIDbContext> dbContextProvider):base(dbContextProvider){}

public async Task<Product?> FindByNameAsync(string name)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(p=>p.ProductName==name);
        }

    }
}
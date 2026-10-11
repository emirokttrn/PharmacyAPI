using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmacyAPI.EntityFrameworkCore;
using PharmacyAPI.IRepositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PharmacyAPI.Reviews
{
    public class ReviewRepository : EfCoreRepository<PharmacyAPIDbContext, Review, Guid>, IReviewRepository
    {
        public ReviewRepository(IDbContextProvider<PharmacyAPIDbContext> dbContextProvider) : base(dbContextProvider) { }

        public async Task<Review?> FindByProductAndCustomerAsync(Guid productId, Guid customerId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(r => r.ProductId == productId && r.CustomerId == customerId);
        }

        public async Task<List<Review>> GetListByProductAsync(Guid productId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.Where(r => r.ProductId == productId).ToListAsync();
        }
    }
}

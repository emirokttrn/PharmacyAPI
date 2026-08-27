using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmacyAPI.EntityFrameworkCore;
using PharmacyAPI.IRepositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PharmacyAPI.Orders
{
    public class OrderRepository : EfCoreRepository<PharmacyAPIDbContext, Order, Guid>, IOrderRepository
    {
        public OrderRepository(IDbContextProvider<PharmacyAPIDbContext> dbContextProvider) : base(dbContextProvider) { }

        public async Task<Order?> GetWithDetailsAsync(Guid id)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.IncludeDetails().FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetListByCustomerAsync(Guid customerId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.IncludeDetails().Where(o => o.CustomerId == customerId).ToListAsync();
        }
    }
}

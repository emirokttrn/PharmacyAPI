using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmacyAPI.EntityFrameworkCore;
using PharmacyAPI.IRepositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PharmacyAPI.CustomerAddresses
{
    public class CustomerAddressRepository : EfCoreRepository<PharmacyAPIDbContext, CustomerAddress, Guid>, ICustomerAddressRepository
    {
        public CustomerAddressRepository(IDbContextProvider<PharmacyAPIDbContext> dbContextProvider) : base(dbContextProvider) { }

        public async Task<List<CustomerAddress>> GetListByCustomerAsync(Guid customerId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.Where(a => a.CustomerId == customerId).ToListAsync();
        }
    }
}

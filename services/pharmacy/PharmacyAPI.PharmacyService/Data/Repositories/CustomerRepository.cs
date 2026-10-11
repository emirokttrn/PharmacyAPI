using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Customers;
using PharmacyAPI.EntityFrameworkCore;
using PharmacyAPI.IRepositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PharmacyAPI
{
    public class CustomerRepository : EfCoreRepository<PharmacyAPIDbContext, Customer, Guid>, ICustomerRepository
    {
        public CustomerRepository(IDbContextProvider<PharmacyAPIDbContext> dbContextProvider) : base(dbContextProvider)
        {
            
        }
        public async Task<Customer?> FindByTcKimlikNoAsync(string tcKimlikNo)
        {
            var DbSet = await GetDbSetAsync();
            return await DbSet.FirstOrDefaultAsync(c=>c.TcKimlikNo==tcKimlikNo);
        }

        public async Task<Customer?> FindByUserIdAsync(Guid userId)
        {
            var DbSet = await GetDbSetAsync();
            return await DbSet.FirstOrDefaultAsync(c=>c.UserId==userId);
        }
    }
}
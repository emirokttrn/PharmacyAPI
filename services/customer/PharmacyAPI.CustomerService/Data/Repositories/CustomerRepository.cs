using Microsoft.EntityFrameworkCore;
using PharmacyAPI.CustomerService.Entities.Customers;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PharmacyAPI.CustomerService.Data.IRepositories
{

    public class CustomerRepository : EfCoreRepository<CustomerServiceDbContext, Customer, Guid>, ICustomerRepository
    {
        public CustomerRepository(IDbContextProvider<CustomerServiceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<Customer?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(cname=>cname.CustomerName==name, cancellationToken);
        }

        public async Task<Customer?> FindByTcKimlikNoAsync(string tcKimlikNo, CancellationToken cancellationToken = default)
        {
           var dbSet = await GetDbSetAsync();
           return await dbSet.FirstOrDefaultAsync(ctc=>ctc.TcKimlikNo==tcKimlikNo, cancellationToken);

        }

        public async Task<Customer?> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
        }
    }
}
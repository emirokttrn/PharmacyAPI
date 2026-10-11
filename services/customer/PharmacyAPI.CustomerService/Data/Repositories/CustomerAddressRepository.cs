

using Microsoft.EntityFrameworkCore;
using PharmacyAPI.CustomerService.Data;
using PharmacyAPI.CustomerService.Entities.CustomersAddresses;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

public class CustomerAddressRepository :EfCoreRepository<CustomerServiceDbContext,CustomerAddress, Guid>,ICustomerAddressRepository
{
  public CustomerAddressRepository(IDbContextProvider<CustomerServiceDbContext> dbContextProvider) : base(dbContextProvider)
  {
  }

    public async Task<List<CustomerAddress>> ListAddressesAsync(Guid customerId)
    {
       var dbSet =await GetDbSetAsync();
       return await dbSet.Where(a => a.CustomerId == customerId).ToListAsync();
    }
}
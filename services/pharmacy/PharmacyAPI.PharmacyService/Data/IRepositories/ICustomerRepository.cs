using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.Customers;
using Volo.Abp.Domain.Repositories;

namespace PharmacyAPI.IRepositories
{
    public interface ICustomerRepository : IRepository<Customer, Guid>
    {
        Task<Customer?> FindByUserIdAsync(Guid userId);
        Task<Customer?> FindByTcKimlikNoAsync(string tcKimlikNo);
    }
}
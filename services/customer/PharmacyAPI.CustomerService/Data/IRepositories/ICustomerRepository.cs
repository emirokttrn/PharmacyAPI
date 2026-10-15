using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PharmacyAPI.CustomerService.Entities.Customers;
using Volo.Abp.Domain.Repositories;

namespace PharmacyAPI
{
    public interface ICustomerRepository : IRepository<Customer, Guid>
    {
        Task<Customer?> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Customer?> FindByTcKimlikNoAsync(string tcKimlikNo, CancellationToken cancellationToken =default);
        Task<Customer?> FindByNameAsync(string name, CancellationToken cancellationToken=default);
    }
}

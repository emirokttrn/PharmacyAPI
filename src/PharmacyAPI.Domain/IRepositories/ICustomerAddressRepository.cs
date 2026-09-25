using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyAPI.CustomerAddresses;
using Volo.Abp.Domain.Repositories;

namespace PharmacyAPI.IRepositories
{
    public interface ICustomerAddressRepository : IRepository<CustomerAddress, Guid>
    {
        Task<List<CustomerAddress>> GetListByCustomerAsync(Guid customerId);
    }
}

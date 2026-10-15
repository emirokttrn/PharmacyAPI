using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyAPI.CustomerService.Entities.CustomerAddresses;
using Volo.Abp.Domain.Repositories;

public interface ICustomerAddressRepository : IRepository<CustomerAddress, Guid>
{
    Task<List<CustomerAddress>> ListAddressesAsync(Guid customerId);
}
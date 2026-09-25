using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyAPI.CustomerAddressDtos;
using Volo.Abp.Application.Services;

namespace PharmacyAPI.IServices.CustomerAddress
{
    public interface ICustomerAddressService : IApplicationService
    {
        Task<CustomerAddressDto> CreateAsync(CreateCustomerAddressDto request);
        Task<CustomerAddressDto> GetAsync(Guid id);
        Task<List<CustomerAddressDto>> GetListByCustomerAsync(Guid customerId);
        Task<CustomerAddressDto> SetDefaultAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}

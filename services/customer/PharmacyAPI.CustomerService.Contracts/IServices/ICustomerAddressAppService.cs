

using PharmacyAPI.CustomerService.Contracts.Dtos.CustomerAddresses;
using Volo.Abp.Application.Services;

public interface ICustomerAddressAppService : IApplicationService
{
    Task<CustomerAddressDto> CreateCustomerAddressAsync(CreateCustomerAddressDto input);
    Task<CustomerAddressDto> GetCustomerAdressAsync(Guid id);
    Task<List<CustomerAddressDto>> GetAllCustomerAddressesAsync();
    Task<CustomerAddressDto> UpdateCustomerAddressAsync(Guid id, CreateCustomerAddressDto input);
    Task DeleteCustomerAddressAsync(Guid id);
}
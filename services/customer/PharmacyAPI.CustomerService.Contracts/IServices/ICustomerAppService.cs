

using PharmacyAPI.CustomerService.Contracts.Dtos.Customers;
using Volo.Abp.Application.Services;

public interface ICustomerAppService : IApplicationService
{
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto input);
    Task<CustomerDto> GetCustomerAsync(Guid id);
    Task<string> GetCustomerTcKimlikNoAsync(Guid id);
    Task<List<CustomerDto>> GetAllCustomersAsync();
    Task<CustomerDto> UpdateCustomerAsync(Guid id,UpdateCustomerDto input);
    Task DeleteCustomerAsync(Guid id);

}
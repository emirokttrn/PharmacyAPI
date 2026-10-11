using PharmacyAPI.CustomerService.Contracts.Dtos.Integration;
using Volo.Abp.Application.Services;

namespace PharmacyAPI.CustomerService.Contracts.IServices;

// Diger servislerin (orn. siparis) musteri bilgisine ihtiyac duydugu servisler arasi uc nokta.
public interface ICustomerIntegrationService : IApplicationService
{
    Task<CustomerOrderContextDto> GetOrderContextAsync(Guid customerId, Guid shippingAddressId);
}

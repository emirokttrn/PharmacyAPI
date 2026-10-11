using PharmacyAPI.CustomerService.Contracts.Dtos.Integration;
using PharmacyAPI.CustomerService.Contracts.IServices;
using PharmacyAPI.CustomerService.Entities.Customers;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace PharmacyAPI.CustomerService.Services.Integration
{
    public class CustomerIntegrationService : ApplicationService, ICustomerIntegrationService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerAddressRepository _customerAddressRepository;

        public CustomerIntegrationService(
            ICustomerRepository customerRepository,
            ICustomerAddressRepository customerAddressRepository)
        {
            _customerRepository = customerRepository;
            _customerAddressRepository = customerAddressRepository;
        }

        // KVKK veri minimizasyonu: TC Kimlik No ve dogum tarihi DTO'ya konmaz, sadece IsAdult doner.
        public async Task<CustomerOrderContextDto> GetOrderContextAsync(Guid customerId, Guid shippingAddressId)
        {
            var customer = await _customerRepository.FindAsync(customerId);
            if (customer == null)
            {
                throw new UserFriendlyException("Boyle bir musteri yok.");
            }

            var address = await _customerAddressRepository.FindAsync(shippingAddressId);
            if (address == null || address.CustomerId != customerId)
            {
                throw new UserFriendlyException("Teslimat adresi bulunamadi ya da bu musteriye ait degil.");
            }

            return new CustomerOrderContextDto
            {
                IsAdult = customer.DateOfBirth.HasValue && Customer.CalculateAge(customer.DateOfBirth.Value) >= 18,
                CustomerName = customer.CustomerName,
                PhoneNumber = customer.PhoneNumber,
                AddressTitle = address.Title,
                FullAddress = address.FullAddress,
                City = address.City,
                District = address.District,
                PostalCode = address.PostalCode
            };
        }
    }
}

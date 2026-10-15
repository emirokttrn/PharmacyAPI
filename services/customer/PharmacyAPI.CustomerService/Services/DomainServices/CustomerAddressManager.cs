using System;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.CustomerService.Data.IRepositories;
using PharmacyAPI.CustomerService.Entities.CustomerAddresses;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace PharmacyAPI.CustomerService.Services.DomainServices
{
    public class CustomerAddressManager : DomainService
    {
        private readonly ICustomerAddressRepository _customerAddressRepository;
        private readonly ICustomerRepository _customerRepository;

        public CustomerAddressManager(
            ICustomerAddressRepository customerAddressRepository,
            ICustomerRepository customerRepository)
        {
            _customerAddressRepository = customerAddressRepository;
            _customerRepository = customerRepository;
        }

        public async Task<CustomerAddress> CreateAsync(
            Guid customerId,
            string title,
            string fullAddress,
            string city,
            string district,
            string? postalCode,
            bool isDefault)
        {
            var customer = await _customerRepository.FindAsync(customerId);
            if (customer == null)
            {
                throw new UserFriendlyException("Boyle bir musteri yok.");
            }

            var address = new CustomerAddress(GuidGenerator.Create(), customerId, title, fullAddress, city, district, postalCode);

            var existingAddresses = await _customerAddressRepository.ListAddressesAsync(customerId);

            // Ilk adres her zaman varsayilan olur; aksi halde istenirse varsayilan yapilir
            if (isDefault || existingAddresses.Count == 0)
            {
                foreach (var existing in existingAddresses.Where(a => a.IsDefault))
                {
                    existing.UnmarkAsDefault();
                    await _customerAddressRepository.UpdateAsync(existing);
                }

                address.MarkAsDefault();
            }

            return address;
        }

        public async Task SetDefaultAsync(Guid customerId, Guid addressId)
        {
            var addresses = await _customerAddressRepository.ListAddressesAsync(customerId);

            var target = addresses.FirstOrDefault(a => a.Id == addressId);
            if (target == null)
            {
                throw new UserFriendlyException("Bu adres bu musteriye ait degil.");
            }

            foreach (var address in addresses.Where(a => a.IsDefault && a.Id != addressId))
            {
                address.UnmarkAsDefault();
                await _customerAddressRepository.UpdateAsync(address);
            }

            target.MarkAsDefault();
            await _customerAddressRepository.UpdateAsync(target);
        }
    }
}
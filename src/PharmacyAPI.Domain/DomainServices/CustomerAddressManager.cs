using System;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.CustomerAddresses;
using PharmacyAPI.IRepositories;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace PharmacyAPI.DomainServices
{
    // [Claude Agent] - Adres olusturma + "her musterinin en fazla 1 IsDefault=true adresi olabilir" kurali
    public class CustomerAddressManager : DomainService
    {
        private readonly ICustomerAddressRepository _customerAddressRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IGuidGenerator _guidGenerator;

        public CustomerAddressManager(
            ICustomerAddressRepository customerAddressRepository,
            ICustomerRepository customerRepository,
            IGuidGenerator guidGenerator)
        {
            _customerAddressRepository = customerAddressRepository;
            _customerRepository = customerRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task<CustomerAddress> CreateAsync(Guid customerId, string title, string fullAddress, string city, string district)
        {
            var customer = await _customerRepository.FindAsync(customerId);
            if (customer == null)
            {
                throw new UserFriendlyException("boyle bir musteri yok!");
            }

            return new CustomerAddress(_guidGenerator.Create(), customerId, title, fullAddress, city, district);
        }

        public async Task SetDefaultAsync(Guid customerId, Guid addressId)
        {
            var addresses = await _customerAddressRepository.GetListByCustomerAsync(customerId);
            var target = addresses.FirstOrDefault(a => a.Id == addressId);
            if (target == null)
            {
                throw new UserFriendlyException("bu adres bu musteriye ait degil!");
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

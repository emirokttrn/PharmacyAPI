using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyAPI.CustomerAddressDtos;
using PharmacyAPI.CustomerAddresses;
using PharmacyAPI.DomainServices;
using PharmacyAPI.IRepositories;
using PharmacyAPI.IServices.CustomerAddress;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace PharmacyAPI.CustomerAddressServices
{
    public class CustomerAddressService : ApplicationService, ICustomerAddressService
    {
        private readonly ICustomerAddressRepository _repository;
        private readonly CustomerAddressManager _customerAddressManager;

        public CustomerAddressService(ICustomerAddressRepository repository, CustomerAddressManager customerAddressManager)
        {
            _repository = repository;
            _customerAddressManager = customerAddressManager;
        }

        public async Task<CustomerAddressDto> CreateAsync(CreateCustomerAddressDto request)
        {
            var address = await _customerAddressManager.CreateAsync(
                request.CustomerId, request.Title, request.FullAddress, request.City, request.District);

            address.SetPostalCode(request.PostalCode);

            // [Claude Agent] - autoSave:true sart: IsDefault=true ise hemen altta SetDefaultAsync
            // bu adresi customerId'ye gore DB'den tekrar sorguluyor (GetListByCustomerAsync).
            // autoSave olmadan insert henuz flush edilmedigi icin o sorgu adresi bulamiyor ve
            // "bu adres bu musteriye ait degil!" hatasi firliyordu (ozellikle bir musterinin ilk
            // adresini IsDefault=true ile eklerken).
            await _repository.InsertAsync(address, autoSave: true);

            if (request.IsDefault)
            {
                await _customerAddressManager.SetDefaultAsync(request.CustomerId, address.Id);
            }

            return ObjectMapper.Map<CustomerAddress, CustomerAddressDto>(address);
        }

        public async Task<CustomerAddressDto> GetAsync(Guid id)
        {
            var address = await _repository.GetAsync(id);
            return ObjectMapper.Map<CustomerAddress, CustomerAddressDto>(address);
        }

        public async Task<List<CustomerAddressDto>> GetListByCustomerAsync(Guid customerId)
        {
            var addresses = await _repository.GetListByCustomerAsync(customerId);
            return ObjectMapper.Map<List<CustomerAddress>, List<CustomerAddressDto>>(addresses);
        }

        public async Task<CustomerAddressDto> SetDefaultAsync(Guid id)
        {
            var address = await _repository.GetAsync(id);
            await _customerAddressManager.SetDefaultAsync(address.CustomerId, id);
            var updated = await _repository.GetAsync(id);
            return ObjectMapper.Map<CustomerAddress, CustomerAddressDto>(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            var address = await _repository.FindAsync(id);
            if (address == null)
            {
                throw new UserFriendlyException("bu id ile bir adres bulunamadi");
            }
            await _repository.DeleteAsync(id);
        }
    }
}

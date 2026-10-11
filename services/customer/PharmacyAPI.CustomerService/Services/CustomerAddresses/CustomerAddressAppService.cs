using PharmacyAPI;
using PharmacyAPI.CustomerService.Contracts.Dtos.CustomerAddresses;
using PharmacyAPI.CustomerService.Entities.CustomersAddresses;
using PharmacyAPI.CustomerService.Services.DomainServices;
using Volo.Abp;
using Volo.Abp.Application.Services;



public class CustomerAddressAppService : ApplicationService, ICustomerAddressAppService
{
    private readonly ICustomerAddressRepository _customerAddressRepository;

    private readonly CustomerAddressManager _customerAddressManager;
         public CustomerAddressAppService(ICustomerAddressRepository repository, CustomerAddressManager customerAddressManager)
        {
            _customerAddressRepository = repository;
            _customerAddressManager = customerAddressManager;
        }
    public async Task<CustomerAddressDto> CreateCustomerAddressAsync(CreateCustomerAddressDto input)
    {
        var customerAddress = await _customerAddressManager.CreateAsync(input.CustomerId, input.Title, input.FullAddress, input.City, input.District, input.PostalCode, input.IsDefault);
        await _customerAddressRepository.InsertAsync(customerAddress, autoSave: true);
        return ObjectMapper.Map<CustomerAddress, CustomerAddressDto>(customerAddress);
    }



    public async Task DeleteCustomerAddressAsync(Guid id)
    {
        var findCustomerAddress = await _customerAddressRepository.FindAsync(id);
        if (findCustomerAddress != null)
        {
            await _customerAddressRepository.DeleteAsync(findCustomerAddress);
        }else
        {
            throw new UserFriendlyException("Boyle bir adres yok.");
        }
        }

    public Task<List<CustomerAddressDto>> GetAllCustomerAddressesAsync()
    {
        var customerAddresses = _customerAddressRepository.GetListAsync();
        return ObjectMapper.Map<Task<List<CustomerAddress>>, Task<List<CustomerAddressDto>>>(customerAddresses);
    }

    public Task<CustomerAddressDto> GetCustomerAdressAsync(Guid id)
    {
        var customerAddress = _customerAddressRepository.GetAsync(id);
        if(customerAddress == null)
        {
            throw new UserFriendlyException("Boyle bir adres yok.");
        }
        return ObjectMapper.Map<Task<CustomerAddress>, Task<CustomerAddressDto>>(customerAddress);
    }

    public Task<CustomerAddressDto> UpdateCustomerAddressAsync(Guid id, CreateCustomerAddressDto input)
    {
        var customerAddress = _customerAddressRepository.GetAsync(id);
        if(customerAddress == null)
        {
            throw new UserFriendlyException("Boyle bir adres yok.");
        }
        customerAddress.Result.SetTitle(input.Title);
        customerAddress.Result.SetFullAddress(input.FullAddress);
        customerAddress.Result.SetCity(input.City);
        customerAddress.Result.SetDistrict(input.District);
        customerAddress.Result.SetPostalCode(input.PostalCode);
        return ObjectMapper.Map<Task<CustomerAddress>, Task<CustomerAddressDto>>(customerAddress);
    }
}
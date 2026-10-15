using PharmacyAPI.ConsentRecords;
using PharmacyAPI.CustomerService.Contracts.Dtos.Customers;
using PharmacyAPI.CustomerService.Data.IRepositories;
using PharmacyAPI.CustomerService.Entities.Customers;
using PharmacyAPI.CustomerService.Services.DomainServices;
using Volo.Abp.Application.Services;
using Volo.Abp.Security.Encryption;
using Volo.Abp.Users;

namespace PharmacyAPI.CustomerService.Services.Customers
{
    public class CustomerAppService : ApplicationService, ICustomerAppService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IConsentRecordRepository _consentRecordRepository;
        private readonly CustomerManager _customerManager;
        private readonly IStringEncryptionService _stringEncryptionService;

        public CustomerAppService(
            ICustomerRepository customerRepository,
            IConsentRecordRepository consentRecordRepository,
            CustomerManager customerManager,
            IStringEncryptionService stringEncryptionService)
        {
            _customerRepository = customerRepository;
            _consentRecordRepository = consentRecordRepository;
            _customerManager = customerManager;
            _stringEncryptionService = stringEncryptionService;
        }

        // Musteri profili, istegi atan kullanicinin kendisi icin olusturulur (UserId token'dan gelir).
        public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto input)
        {
            var consentInputs = input.Consents
                .Select(c => new ConsentInput(c.ConsentType, c.ConsentGiven, c.ConsentTextVersion, c.IpAddress))
                .ToList();

            var (customer, consents) = await _customerManager.CreateAsync(
                CurrentUser.GetId(),
                input.Name,
                input.PhoneNumber,
                input.TcKimlikNo,
                consentInputs);

            if (input.Address != null)
            {
                customer.SetAddress(input.Address);
            }
            customer.SetDateOfBirth(input.DateOfBirth);

            await _customerRepository.InsertAsync(customer);
            await _consentRecordRepository.InsertManyAsync(consents);

            return MapToMaskedDto(customer);
        }

        public async Task<CustomerDto> GetCustomerAsync(Guid id)
        {
            var customer = await _customerRepository.GetAsync(id);
            return MapToMaskedDto(customer);
        }

        // KVKK: tam TC Kimlik No sadece bu ayri endpoint'te doner.
        public async Task<string> GetCustomerTcKimlikNoAsync(Guid id)
        {
            var customer = await _customerRepository.GetAsync(id);
            return _stringEncryptionService.Decrypt(customer.TcKimlikNo)!;
        }

        public async Task<List<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetListAsync();
            return customers.Select(MapToMaskedDto).ToList();
        }

        public async Task<CustomerDto> UpdateCustomerAsync(Guid id, UpdateCustomerDto input)
        {
            var customer = await _customerRepository.GetAsync(id);

            customer.SetName(input.Name);
            customer.SetPhoneNumber(input.PhoneNumber);
            if (input.Address != null)
            {
                customer.SetAddress(input.Address);
            }
            customer.SetDateOfBirth(input.DateOfBirth);

            await _customerRepository.UpdateAsync(customer);

            return MapToMaskedDto(customer);
        }

        public async Task DeleteCustomerAsync(Guid id)
        {
            await _customerRepository.GetAsync(id);
            await _customerRepository.DeleteAsync(id);
        }

        // KVKK: normal DTO cikislarinda TC Kimlik No maskelenir (orn. "123****8901").
        private CustomerDto MapToMaskedDto(Customer customer)
        {
            var dto = ObjectMapper.Map<Customer, CustomerDto>(customer);
            dto.TcKimlikNo = MaskTcKimlikNo(customer.TcKimlikNo);
            return dto;
        }

        private string MaskTcKimlikNo(string encryptedTcKimlikNo)
        {
            var plain = _stringEncryptionService.Decrypt(encryptedTcKimlikNo);
            if (string.IsNullOrEmpty(plain) || plain.Length < 7)
            {
                return new string('*', plain?.Length ?? 0);
            }
            return plain.Substring(0, 3) + "****" + plain.Substring(plain.Length - 4);
        }
    }
}

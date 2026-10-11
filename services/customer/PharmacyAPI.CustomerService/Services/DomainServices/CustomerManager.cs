using PharmacyAPI.ConsentRecords;
using PharmacyAPI.CustomerService.Entities.Customers;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Security.Encryption;

namespace PharmacyAPI.CustomerService.Services.DomainServices
{
    public class CustomerManager : DomainService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IStringEncryptionService _stringEncryptionService;

        public CustomerManager(
            ICustomerRepository customerRepository,
            IStringEncryptionService stringEncryptionService)
        {
            _customerRepository = customerRepository;
            _stringEncryptionService = stringEncryptionService;
        }

        public async Task<(Customer Customer, List<ConsentRecord> Consents)> CreateAsync(
            Guid userId,
            string name,
            string phoneNumber,
            string tcKimlikNo,
            List<ConsentInput> consents)
        {
            var alreadyCustomer = await _customerRepository.FindByUserIdAsync(userId);
            if (alreadyCustomer != null)
            {
                throw new UserFriendlyException("Bu kullanici zaten bir musteri profiline sahip.");
            }

            if (!Customer.IsValidTcKimlikNoFormat(tcKimlikNo))
            {
                throw new UserFriendlyException("Gecerli bir TC Kimlik No degil.");
            }

            var encryptedTc = _stringEncryptionService.Encrypt(tcKimlikNo)!;

            var tcAlreadyUsed = await _customerRepository.FindByTcKimlikNoAsync(encryptedTc);
            if (tcAlreadyUsed != null)
            {
                throw new UserFriendlyException("Bu TC zaten sisteme kayitli.");
            }

            var requiredConsents = new[] { ConsentType.KvkkAydinlatma, ConsentType.AcikRiza };
            foreach (var requiredType in requiredConsents)
            {
                var given = consents != null && consents.Any(c => c.ConsentType == requiredType && c.ConsentGiven);
                if (!given)
                {
                    throw new UserFriendlyException($"{requiredType} rizasi verilmeden musteri kaydi olusturulamaz.");
                }
            }

            var customer = new Customer(GuidGenerator.Create(), userId, name, phoneNumber, encryptedTc);

            var consentRecords = (consents ?? new List<ConsentInput>())
                .Select(c => new ConsentRecord(
                    GuidGenerator.Create(),
                    customer.Id,
                    c.ConsentType,
                    c.ConsentGiven,
                    c.ConsentTextVersion,
                    c.IpAddress))
                .ToList();

            return (customer, consentRecords);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.ConsentRecords;
using PharmacyAPI.Customers;
using PharmacyAPI.IRepositories;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Security.Encryption;

namespace PharmacyAPI.DomainServices
{
    public class CustomerManager : DomainService
    {
        private readonly ICustomerRepository _customerepository;
        private readonly IIdentityUserRepository _ıdentityUserRepository;
        private readonly IStringEncryptionService _stringEncryptionService;
        private readonly IGuidGenerator _guidGenerator;

        public CustomerManager(
                    ICustomerRepository customerRepository,
                    IIdentityUserRepository identityUserRepository,
                    IStringEncryptionService stringEncryptionService,
                    IGuidGenerator guidGenerator)
        {
            _customerepository = customerRepository;
            _ıdentityUserRepository = identityUserRepository;
            _stringEncryptionService = stringEncryptionService;
            _guidGenerator = guidGenerator;
        }


        // [Claude Agent] - TC Kimlik No artik veritabanina yazilmadan once IStringEncryptionService ile
        // sifreleniyor (ABP'nin varsayilan sifrelemesi deterministic oldugu icin ayni TC her zaman ayni
        // ciphertext'i uretir, bu sayede FindByTcKimlikNoAsync sifreli deger uzerinden de calisir).
        // KVKK: en az KvkkAydinlatma ve AcikRiza rizasi verilmis olmali, verilmemisse hata.
        public async Task<(Customer Customer, List<ConsentRecord> Consents)> CreateAsync(Guid UserId, string tcKimlikNo, List<ConsentInput> consents)
        {
            await _ıdentityUserRepository.GetAsync(UserId);

            var alreadyCustomer = await _customerepository.FindByUserIdAsync(UserId);
            if(alreadyCustomer !=null)
            {
                throw new UserFriendlyException("bu kullanii zaten bi musteri profiline sahip");
            }

            if (!Customer.IsValidTcKimlikNoFormat(tcKimlikNo))
            {
                throw new UserFriendlyException($"{tcKimlikNo} gecerli bir TC Kimlik No degil.");
            }

            var encryptedTc = _stringEncryptionService.Encrypt(tcKimlikNo);

            var TcAlreadyUsed = await _customerepository.FindByTcKimlikNoAsync(encryptedTc);
            if(TcAlreadyUsed !=null)
            {
                throw new UserFriendlyException("bu tc zaten sisteme kayitli");
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

            var customer = new Customer(_guidGenerator.Create(), UserId, encryptedTc);

            var consentRecords = (consents ?? new List<ConsentInput>())
                .Select(c => new ConsentRecord(_guidGenerator.Create(), customer.Id, c.ConsentType, c.ConsentGiven, c.ConsentTextVersion, c.IpAddress))
                .ToList();

            return (customer, consentRecords);
        }


    }

}
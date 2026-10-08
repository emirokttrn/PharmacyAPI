using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.ConsentRecords;
using PharmacyAPI.customerdtos;
using PharmacyAPI.Customers;
using PharmacyAPI.DomainServices;
using PharmacyAPI.IRepositories;
using PharmacyAPI.IServices.customer;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using System.Linq.Dynamic.Core;
using PharmacyAPI;
using Volo.Abp.Security.Encryption;

namespace PharmacyAPI.CustomerServices
{
    public class CustomerService : ApplicationService, ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IConsentRecordRepository _consentRecordRepository;
        private readonly CustomerManager _manager;
        private readonly IStringEncryptionService _stringEncryptionService;

        public CustomerService(
            ICustomerRepository repository,
            IConsentRecordRepository consentRecordRepository,
            CustomerManager manager,
            IStringEncryptionService stringEncryptionService)
        {
            _manager = manager;
            _repository = repository;
            _consentRecordRepository = consentRecordRepository;
            _stringEncryptionService = stringEncryptionService;
        }

        public async Task<CustomerDto> CreateAsync(CreateCustomerDto request)
        {
            var consentInputs = request.Consents
                .Select(c => new ConsentInput(c.ConsentType, c.ConsentGiven, c.ConsentTextVersion, c.IpAddress))
                .ToList();

            var (customer, consents) = await _manager.CreateAsync(request.UserId, request.TcKimlikNo, consentInputs);

            if (request.Address != null)
            {
                customer.SetAddress(request.Address);
            }
            if (request.DateofBirth != null)
            {
                customer.SetDateOfBirth(request.DateofBirth);
            }

            await _repository.InsertAsync(customer);

            foreach (var consent in consents)
            {
                await _consentRecordRepository.InsertAsync(consent);
            }

            return MapToMaskedDto(customer);
        }

        // [Claude Agent] - KVKK: tam TC Kimlik No sadece bu ayri endpoint'te donuyor (yetkilendirme kapsam disi).
        public async Task<string> GetTcKimlikNoAsync(Guid id)
        {
            var customer = await _repository.GetAsync(id);
            return _stringEncryptionService.Decrypt(customer.TcKimlikNo);
        }

        // [Claude Agent] - Normal DTO cikislarinda TC Kimlik No maskelenerek gosterilir (orn. "123****8901")
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

        public async Task DeleteAsync(Guid id)
        {
            var customer = await _repository.FindAsync(id);
            if (customer == null)
            {
                throw new UserFriendlyException("bu id ile bir musteri bulunamadi");
            }
            await _repository.DeleteAsync(id);
        }

        public async Task<PagedResultDto<CustomerDto>> FilterAsync(FilterCustomerDto filter)
        {
            if (filter.Sorting.IsNullOrWhiteSpace())
            {
                filter.Sorting = nameof(Customer.UserId);
            }

            var queryable = await _repository.GetQueryableAsync();

            if (!filter.tcKimlikNo.IsNullOrWhiteSpace())
            {
                // [Claude Agent] - TC artik sifreli saklandigi icin filtre degeri de sifrelenip oyle karsilastiriliyor
                var encryptedFilterTc = _stringEncryptionService.Encrypt(filter.tcKimlikNo);
                queryable = queryable.Where(c => c.TcKimlikNo == encryptedFilterTc);
            }

            var totalCount = queryable.Count();

            var result = await AsyncExecuter.ToListAsync(
                queryable
                    .OrderBy(filter.Sorting)
                    .Skip(filter.SkipCount)
                    .Take(filter.MaxResultCount)
            );

            var dtos = await MapToDtosAsync(result);
            return new PagedResultDto<CustomerDto>(totalCount, dtos);
        }

        // N+1 uyarisi: her Customer icin ayri ayri IdentityUser cekiliyor.
        // Kucuk sayfa boyutlarinda (varsayilan MaxResultCount) sorun degil,
        // buyuk listelerde performans icin toplu (batch) sorguya cevrilebilir.
        private async Task<List<CustomerDto>> MapToDtosAsync(List<Customer> customers)
        {
            var dtos = new List<CustomerDto>();
            foreach (var customer in customers)
            {
                dtos.Add(MapToMaskedDto(customer));
            }
            return dtos;
        }

        public async Task<CustomerDto> GetAsync(Guid id)
        {
            var customer = await _repository.GetAsync(id);
            return MapToMaskedDto(customer);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.customerdtos;
using PharmacyAPI.Customers;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace PharmacyAPI.CustomerServices
{
    // Sadece DTO tarafindaki eslenmeyen alanlar uyari verir; entity'deki ConcurrencyStamp,
    // ExtraProperties ve soft-delete alanlari DTO'da bilerek yok.
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CustomerApplicationMapper : MapperBase<Customer, CustomerDto>
    {
        // [Claude Agent] - Entity'deki "DateOfBirth" (buyuk O) ile DTO'daki "DateofBirth" (kucuk o)
        // property adlari birebir eslesmedigi icin Mapperly bu alani sessizce atliyordu; GET
        // cevaplarinda dogum tarihi her zaman null donuyordu. Acikca eslestiriyoruz.
        // Iki overload'da da gerekli; attribute sadece uzerine yazildigi metoda uygulanir.
        [MapProperty(nameof(Customer.DateOfBirth), nameof(CustomerDto.DateofBirth))]
        public override partial CustomerDto Map(Customer source);
        [MapProperty(nameof(Customer.DateOfBirth), nameof(CustomerDto.DateofBirth))]
        public override partial void Map(Customer source, CustomerDto destination);

    }
}
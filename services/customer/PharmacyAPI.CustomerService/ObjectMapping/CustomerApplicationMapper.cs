using PharmacyAPI.CustomerService.Contracts.Dtos.Customers;
using PharmacyAPI.CustomerService.Entities.Customers;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace PharmacyAPI.CustomerService.ObjectMapping;

// Sadece DTO tarafindaki eslenmeyen alanlar uyari verir; entity'deki ConcurrencyStamp
// ve ExtraProperties DTO'da bilerek yok.
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CustomerApplicationMapper : MapperBase<Customer, CustomerDto>
{
    // Entity'de "CustomerName", DTO'da "Name"; adlar farkli oldugu icin acikca eslestiriyoruz.
    // Attribute sadece uzerine yazildigi metoda uygulanir, iki overload'da da gerekli.
    [MapProperty(nameof(Customer.CustomerName), nameof(CustomerDto.Name))]
    public override partial CustomerDto Map(Customer source);
    [MapProperty(nameof(Customer.CustomerName), nameof(CustomerDto.Name))]
    public override partial void Map(Customer source, CustomerDto destination);
}

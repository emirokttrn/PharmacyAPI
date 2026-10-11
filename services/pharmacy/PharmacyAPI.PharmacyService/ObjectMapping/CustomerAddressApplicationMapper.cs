using PharmacyAPI.CustomerAddressDtos;
using PharmacyAPI.CustomerAddresses;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace PharmacyAPI.CustomerAddressServices
{
    // Sadece DTO tarafindaki eslenmeyen alanlar uyari verir; entity'deki ConcurrencyStamp
    // ve ExtraProperties DTO'da bilerek yok.
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CustomerAddressApplicationMapper : MapperBase<CustomerAddress, CustomerAddressDto>
    {
        public override partial CustomerAddressDto Map(CustomerAddress source);
        public override partial void Map(CustomerAddress source, CustomerAddressDto destination);
    }
}

using PharmacyAPI.CustomerAddressDtos;
using PharmacyAPI.CustomerAddresses;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace PharmacyAPI.CustomerAddressServices
{
    [Mapper]
    public partial class CustomerAddressApplicationMapper : MapperBase<CustomerAddress, CustomerAddressDto>
    {
        public override partial CustomerAddressDto Map(CustomerAddress source);
        public override partial void Map(CustomerAddress source, CustomerAddressDto destination);
    }
}

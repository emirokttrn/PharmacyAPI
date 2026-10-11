using PharmacyAPI.OrderDtos;
using PharmacyAPI.Orders;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace PharmacyAPI.OrderServices
{
    [Mapper]
    public partial class OrderApplicationMapper : MapperBase<Order, OrderDto>
    {
        public override partial OrderDto Map(Order source);
        public override partial void Map(Order source, OrderDto destination);
    }
}

using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace PharmacyAPI.OrderDtos
{
    public class OrderLineDto : EntityDto<Guid>
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class OrderDto : FullAuditedEntityDto<Guid>
    {
        public Guid CustomerId { get; set; }
        public Guid ShippingAddressId { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public List<OrderLineDto> OrderLines { get; set; } = new List<OrderLineDto>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PharmacyAPI.Orders
{
    // [Claude Agent] - Order/Cart aggregate root'u. OrderLine'lar Order icinde yasar, ayri
    // repository'leri yok. Stok/recete/yas kontrolleri OrderManager'da yapilir.
    public static class OrderConsts
    {
        public const int TrackingNumberMaxLength = 64;
    }

    public class Order : FullAuditedAggregateRoot<Guid>
    {
        public Guid CustomerId { get; protected set; }
        public Guid ShippingAddressId { get; protected set; }
        public OrderStatus Status { get; protected set; }
        public decimal TotalAmount { get; protected set; }
        public string? TrackingNumber { get; protected set; }
        public DateTime? ShippedDate { get; protected set; }
        public DateTime? DeliveredDate { get; protected set; }

        public ICollection<OrderLine> OrderLines { get; protected set; } = new List<OrderLine>();

        protected Order() { }

        internal Order(Guid id, Guid customerId, Guid shippingAddressId) : base(id)
        {
            CustomerId = customerId;
            ShippingAddressId = shippingAddressId;
            Status = OrderStatus.Pending;
        }

        internal void AddLine(OrderLine line)
        {
            OrderLines.Add(line);
            RecalculateTotal();
        }

        private void RecalculateTotal()
        {
            TotalAmount = OrderLines.Sum(l => l.UnitPrice * l.Quantity);
        }

        public void SetTrackingNumber(string? trackingNumber)
        {
            TrackingNumber = Check.Length(trackingNumber, nameof(TrackingNumber), OrderConsts.TrackingNumberMaxLength);
        }

        // [Claude Agent] - Siparis durumu sadece Pending->Confirmed->Shipped->Delivered sirasinda
        // ilerleyebilir; Cancelled sadece Pending/Confirmed durumundan yapilabilir.
        public void SetStatus(OrderStatus newStatus)
        {
            if (Status == OrderStatus.Delivered || Status == OrderStatus.Cancelled)
            {
                throw new UserFriendlyException("bu siparisin durumu artik degistirilemez!");
            }

            if (newStatus == OrderStatus.Cancelled)
            {
                if (Status != OrderStatus.Pending && Status != OrderStatus.Confirmed)
                {
                    throw new UserFriendlyException("sadece Pending veya Confirmed durumundaki siparisler iptal edilebilir!");
                }
            }
            else
            {
                var allowedNext = Status switch
                {
                    OrderStatus.Pending => OrderStatus.Confirmed,
                    OrderStatus.Confirmed => OrderStatus.Shipped,
                    OrderStatus.Shipped => OrderStatus.Delivered,
                    _ => (OrderStatus?)null
                };

                if (allowedNext != newStatus)
                {
                    throw new UserFriendlyException($"{Status} durumundan {newStatus} durumuna gecilemez!");
                }
            }

            Status = newStatus;

            if (newStatus == OrderStatus.Shipped)
            {
                ShippedDate = DateTime.UtcNow;
            }
            else if (newStatus == OrderStatus.Delivered)
            {
                DeliveredDate = DateTime.UtcNow;
            }
        }
    }
}

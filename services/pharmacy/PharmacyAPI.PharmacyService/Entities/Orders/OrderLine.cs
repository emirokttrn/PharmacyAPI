using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace PharmacyAPI.Orders
{
    // [Claude Agent] - Order aggregate'inin parcasi, ayri repository yok. Siparis anindaki
    // urun adi ve fiyati snapshot olarak tutulur, Product degisse bile gecmis siparis etkilenmez.
    public class OrderLine : Entity<Guid>
    {
        public const int ProductNameMaxLength = 128;

        public Guid OrderId { get; protected set; }
        public Guid ProductId { get; protected set; }
        public string ProductName { get; protected set; } = null!;
        public int Quantity { get; protected set; }
        public decimal UnitPrice { get; protected set; }

        protected OrderLine() { }

        internal OrderLine(Guid id, Guid orderId, Guid productId, string productName, int quantity, decimal unitPrice) : base(id)
        {
            OrderId = orderId;
            ProductId = productId;
            SetProductName(productName);
            SetQuantity(quantity);
            SetUnitPrice(unitPrice);
        }

        internal void SetProductName(string productName)
        {
            ProductName = Check.NotNullOrWhiteSpace(productName, nameof(ProductName), ProductNameMaxLength);
        }

        internal void SetQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new UserFriendlyException("siparis adedi 0'dan buyuk olmali!");
            }
            Quantity = quantity;
        }

        internal void SetUnitPrice(decimal unitPrice)
        {
            if (unitPrice < 0)
            {
                throw new UserFriendlyException("birim fiyat negatif olamaz!");
            }
            UnitPrice = unitPrice;
        }
    }
}

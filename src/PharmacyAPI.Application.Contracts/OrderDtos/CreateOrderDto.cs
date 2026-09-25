using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PharmacyAPI.OrderDtos
{
    // [Claude Agent] - Order/Cart olusturma istegi. Fiyat/urun adi snapshot'i OrderManager tarafindan hesaplanir.
    public class CreateOrderLineDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "adet 0'dan buyuk olmali")]
        public int Quantity { get; set; }

        // [Claude Agent] - urun RequiresPrescription=true ise zorunlu, onayli (Approved) olmali
        public Guid? PrescriptionId { get; set; }
    }

    public class CreateOrderDto
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid ShippingAddressId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "siparişte en az bir urun olmali")]
        public List<CreateOrderLineDto> Lines { get; set; } = new List<CreateOrderLineDto>();
    }
}

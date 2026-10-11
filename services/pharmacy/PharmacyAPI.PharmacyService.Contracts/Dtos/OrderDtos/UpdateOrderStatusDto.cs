using System.ComponentModel.DataAnnotations;

namespace PharmacyAPI.OrderDtos
{
    // [Claude Agent] - Personel siparis durumunu Pending -> Confirmed -> Shipped -> Delivered olarak gunceller
    public class UpdateOrderStatusDto
    {
        [Required]
        public OrderStatus Status { get; set; }

        public string? TrackingNumber { get; set; }
    }
}

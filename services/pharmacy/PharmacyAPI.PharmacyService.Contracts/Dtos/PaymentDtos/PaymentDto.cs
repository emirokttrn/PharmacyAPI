using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace PharmacyAPI.PaymentDtos
{

    public class CreatePaymentDto
    {
        [Required]
        public Guid OrderId { get; set; }
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
    }
    public class PaymentDto : EntityDto<Guid>
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? ProviderTransactionId { get; set; }
        public DateTime? PaidAt { get; set; }
        public string? FailureReason { get; set; }
    }

    public class PaymentWebhookDto
    {
        [Required]
        public Guid PaymentId { get; set; }
        [Required]
        public bool IsSuccesful { get; set; }
        public string? ProviderTransactionId { get; set; }
        public string? FailureReason { get; set; }
    }

    public class InitiatePaymentResponseDto
    {
        public Guid PaymentId { get; set; }
        public string CheckoutUrl { get; set; }
    }
    public class RefundPaymentDto
    {
        public required string Reason { get; set; }
    }
}
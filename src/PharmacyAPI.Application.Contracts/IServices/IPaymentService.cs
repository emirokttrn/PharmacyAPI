using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.PaymentDtos;
using Volo.Abp.Application.Services;

namespace PharmacyAPI.IServices
{
    public interface IPaymentService :IApplicationService
    {
        Task<InitiatePaymentResponseDto> CreateAsync(CreatePaymentDto request);
        Task<PaymentDto> GetAsync(Guid id);

        Task<PaymentDto> RefundAsync(Guid id, string reason);
        Task HandleWebhookAsync(PaymentWebhookDto payload);
        Task<bool> VerifyWebhookSignatureAsync(string rawBody, string signature);


    }
}
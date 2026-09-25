using System;
using System.Threading.Tasks;
using PharmacyAPI.Payments;
using PharmacyAPI.PaymentDtos;
using PharmacyAPI.DomainServices;
using Riok.Mapperly.Abstractions;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Mapperly;
using PharmacyAPI.IServices;
using PharmacyAPI.IRepositories;


namespace PharmacyAPI.PaymentServices
{
    public class PaymentService : ApplicationService, IPaymentService
    {

private readonly IPaymentRepository _repository;
        private readonly PaymentManager _paymentManager;
        private readonly IPaymentProviderService _providerService;

        public PaymentService(
            IPaymentRepository repository,
            PaymentManager paymentManager,
            IPaymentProviderService providerService)
        {
            _repository = repository;
            _paymentManager = paymentManager;
            _providerService = providerService;
        }


        public async Task<InitiatePaymentResponseDto> CreateAsync(CreatePaymentDto request)
        {
            var payment = await _paymentManager.CreateAsync(request.OrderId, request.PaymentMethod);
            await _repository.InsertAsync(payment);
            var providerResult = await _providerService.InitiatePaymentAsync(payment.Id, payment.Amount);
            return new InitiatePaymentResponseDto
            {
                PaymentId=payment.Id,
                CheckoutUrl=providerResult.CheckoutUrl
            };
        }

        public async Task<PaymentDto> GetAsync(Guid id)
        {
            var payment = await _repository.GetAsync(id);
            return ObjectMapper.Map<Payment,PaymentDto>(payment);
        }

        public async Task HandleWebhookAsync(PaymentWebhookDto payload)
        {
           if(payload.IsSuccesful)
            {
                await _paymentManager.COnfirmSuccessAsync(payload.PaymentId,payload.ProviderTransactionId ?? string.Empty);
            }
            else
            {
                await _paymentManager.ConfirmFailureAsync(payload.PaymentId, payload.FailureReason ?? "Bilinmeyen hata");
            }
        }

        public async Task<PaymentDto> RefundAsync(Guid id, string reason)
        {
        var  payment = await _paymentManager.RefundAsync(id,reason);
        return ObjectMapper.Map<Payment,PaymentDto>(payment);
        }
        public Task<bool> VerifyWebhookSignatureAsync(string rawBody, string signature)
{
    return Task.FromResult(_providerService.VerifyWebhookSignature(rawBody, signature));
}  

    }
               [Mapper]
    public partial class PaymentApplicationMapper : MapperBase<Payment, PaymentDto>
    {
        public override partial PaymentDto Map(Payment source);
        public override partial void Map(Payment source, PaymentDto destination);
    }
}
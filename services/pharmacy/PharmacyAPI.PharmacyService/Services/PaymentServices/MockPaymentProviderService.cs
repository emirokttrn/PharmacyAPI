using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace PharmacyAPI.PaymentServices
{
   public class InitiatePaymentResult
    {
        public string CheckoutUrl { get; set; }
        public string ProviderReferenceId { get; set; }
    }

    public interface IPaymentProviderService
    {
        Task<InitiatePaymentResult> InitiatePaymentAsync(Guid paymentId, decimal amount);
        bool VerifyWebhookSignature(string payload, string signatureHeader);
    }

    // Gercek iyzico entegrasyonu ileride bu interface'in baska bir implementasyonu
    // olarak eklenecek - simdilik sahte (mock)
    public class MockPaymentProviderService : IPaymentProviderService, ITransientDependency
    {
        public Task<InitiatePaymentResult> InitiatePaymentAsync(Guid paymentId, decimal amount)
        {
            return Task.FromResult(new InitiatePaymentResult
            {
                CheckoutUrl = $"https://mock-payment-provider.local/checkout?paymentId={paymentId}&amount={amount}",
                ProviderReferenceId = $"MOCK-{paymentId}"
            });
        }

        public bool VerifyWebhookSignature(string payload, string signatureHeader)
        {
            // Gercek entegrasyonda saglayicinin secret key'iyle HMAC dogrulamasi yapilacak.
            // Mock ortaminda her zaman gecerli sayiyoruz.
            return true;
        }
    }
}
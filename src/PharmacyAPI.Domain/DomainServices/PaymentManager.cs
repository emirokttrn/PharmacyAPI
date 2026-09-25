using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.IRepositories;
using PharmacyAPI.Orders;
using PharmacyAPI.Payments;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace PharmacyAPI.DomainServices
{
    public class PaymentManager : DomainService
    {

        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IGuidGenerator _guidGenerator;

        public PaymentManager(
        IPaymentRepository paymentRepository,
        IOrderRepository orderRepository,
        IGuidGenerator guidGenerator)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task<Payment> CreateAsync(Guid OrderId, PaymentMethod method)
        {
        var order=    await _orderRepository.GetAsync(OrderId);
        var existingPayment = await _paymentRepository.FindOrderIdAsync(OrderId);

        if(existingPayment!=null)
            {
                 throw new UserFriendlyException(
                    "Bu siparis icin zaten bir odeme islemi baslatilmis.");
            }

            return new Payment(_guidGenerator.Create(), OrderId,method,order.TotalAmount);
        }

        public async Task<Payment> COnfirmSuccessAsync(Guid paymentid, string providerTransactionId)
        {
            var payment = await _paymentRepository.GetAsync(paymentid);
            payment.MarkAsSuccesful(providerTransactionId);
            return payment;
        }
            public async Task<Payment> ConfirmFailureAsync(Guid paymentId, string reason)
        {
            var payment = await _paymentRepository.GetAsync(paymentId);
            payment.MarkAsFailed(reason);
            return payment;
        }

        public async Task<Payment> RefundAsync(Guid paymentId, string reason)
        {
            var payment = await _paymentRepository.GetAsync(paymentId);
            payment.MarkAsRefuned(reason);
            return payment;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using PharmacyAPI.Prescriptions;
using PharmacyAPI.Products;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PharmacyAPI.Payments
{
    public class Payment : FullAuditedAggregateRoot<Guid>
    {

        public Guid OrderId { get; protected set; }
        public decimal Amount { get; protected set; }

        public PaymentStatus PaymentStatus {get; protected set;}
        public string? ProviderTransactionId { get; protected set; }
        public PaymentMethod PaymentMethod { get; protected set; }
        public DateTime? PaidAt {get; protected set;}
        public string? FailureReason  {get; protected set;}

protected Payment() {}

internal Payment (Guid id, Guid orderId,  PaymentMethod method,decimal amount):base(id)
        {
            OrderId=orderId;
            AmountSet(amount);
            PaymentMethod=method;
            PaymentStatus=PaymentStatus.Pending;
        }


        internal void AmountSet(decimal amount){
         if(amount<=0)
            {
                throw new UserFriendlyException("miktar 0 veya 0'dan kucuk olamaz");
            }
            Amount = amount;
            
        }
        internal void MarkAsSuccesful(string providerTransactionId)
        {
            if(PaymentStatus!=PaymentStatus.Pending)
            {
                throw new UserFriendlyException($"bu odeme zaten {PaymentStatus} durumunda");
            }
            PaymentStatus=PaymentStatus.Succesful;
            ProviderTransactionId=providerTransactionId;
            PaidAt = DateTime.UtcNow;
        } 
        internal void MarkAsFailed(string reason)
        {
            if(PaymentStatus!=PaymentStatus.Pending)
            {
             throw new UserFriendlyException($"bu odeme zaten {PaymentStatus} durumunda");
            }
            PaymentStatus=PaymentStatus.Failed;
            FailureReason=reason;
        }
            internal void MarkAsRejected(string reason)
        {
            if(PaymentStatus!=PaymentStatus.Pending)
            {
             throw new UserFriendlyException($"bu odeme zaten {PaymentStatus} durumunda");
            }
            PaymentStatus=PaymentStatus.rejected;
            FailureReason=reason;
        }
        internal void MarkAsRefuned(string reason)
        {
             if(PaymentStatus!=PaymentStatus.Succesful)
            {
             throw new UserFriendlyException($"bu odeme zaten {PaymentStatus} durumunda");
            }
            PaymentStatus=PaymentStatus.Refunded;
            FailureReason=reason;
        }


    }

    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.Payments;
using Volo.Abp.Domain.Repositories;

namespace PharmacyAPI.IRepositories
{
    public interface IPaymentRepository : IRepository<Payment, Guid>
    {
        Task<Payment?> FindOrderIdAsync(Guid orderId);
        Task<Payment?> FindByProviderTransactionIdAsync(string providerTransactionId);
    }
}
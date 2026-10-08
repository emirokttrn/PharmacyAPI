using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmacyAPI.EntityFrameworkCore;
using PharmacyAPI.IRepositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PharmacyAPI.Payments
{
    public class PaymentRepository : EfCoreRepository<PharmacyAPIDbContext, Payment, Guid>, IPaymentRepository
    {

        public PaymentRepository(IDbContextProvider<PharmacyAPIDbContext> provider) : base(provider)
        {}

        public async Task<Payment?> FindByProviderTransactionIdAsync(string providerTransactionId)
        {
            var DbSet = await GetDbSetAsync();
            return await DbSet.FirstOrDefaultAsync(p=>p.ProviderTransactionId==providerTransactionId);
        }

        public virtual async Task<Payment?> FindOrderIdAsync(Guid orderId)
        {
          var DbSet = await GetDbSetAsync();
          return await DbSet.FirstOrDefaultAsync(p=>p.OrderId==orderId);
        }
        
    }
}
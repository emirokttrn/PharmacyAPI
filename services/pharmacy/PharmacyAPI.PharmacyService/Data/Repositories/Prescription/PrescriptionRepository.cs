using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmacyAPI.EntityFrameworkCore;
using PharmacyAPI.IRepositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PharmacyAPI.Prescriptions
{
    public class PrescriptionRepository : EfCoreRepository<PharmacyAPIDbContext, Prescription, Guid>, IPrescriptionRepository
    {
        public PrescriptionRepository(IDbContextProvider<PharmacyAPIDbContext> dbContextProvider) : base(dbContextProvider) { }

        public async Task<List<Prescription>> GetListByCustomerAsync(Guid customerId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.Where(p => p.CustomerId == customerId).ToListAsync();
        }
    }
}

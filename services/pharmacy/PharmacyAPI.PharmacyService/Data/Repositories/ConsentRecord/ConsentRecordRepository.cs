using System;
using PharmacyAPI.EntityFrameworkCore;
using PharmacyAPI.IRepositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PharmacyAPI.ConsentRecords
{
    public class ConsentRecordRepository : EfCoreRepository<PharmacyAPIDbContext, ConsentRecord, Guid>, IConsentRecordRepository
    {
        public ConsentRecordRepository(IDbContextProvider<PharmacyAPIDbContext> dbContextProvider) : base(dbContextProvider) { }
    }
}

using PharmacyAPI.ConsentRecords;
using PharmacyAPI.CustomerService.Data.IRepositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PharmacyAPI.CustomerService.Data.Repositories
{
    public class ConsentRecordRepository : EfCoreRepository<CustomerServiceDbContext, ConsentRecord, Guid>, IConsentRecordRepository
    {
        public ConsentRecordRepository(IDbContextProvider<CustomerServiceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}

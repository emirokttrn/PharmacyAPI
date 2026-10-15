
using PharmacyAPI.ConsentRecords;
using Volo.Abp.Domain.Repositories;

namespace PharmacyAPI.CustomerService.Data.IRepositories
{
    public interface IConsentRecordRepository : IRepository<ConsentRecord, Guid>
    {
    }
}
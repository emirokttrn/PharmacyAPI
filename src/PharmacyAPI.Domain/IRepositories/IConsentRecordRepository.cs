using System;
using PharmacyAPI.ConsentRecords;
using Volo.Abp.Domain.Repositories;

namespace PharmacyAPI.IRepositories
{
    public interface IConsentRecordRepository : IRepository<ConsentRecord, Guid>
    {
    }
}

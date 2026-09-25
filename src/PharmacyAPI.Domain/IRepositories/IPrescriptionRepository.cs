using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyAPI.Prescriptions;
using Volo.Abp.Domain.Repositories;

namespace PharmacyAPI.IRepositories
{
    public interface IPrescriptionRepository : IRepository<Prescription, Guid>
    {
        Task<List<Prescription>> GetListByCustomerAsync(Guid customerId);
    }
}

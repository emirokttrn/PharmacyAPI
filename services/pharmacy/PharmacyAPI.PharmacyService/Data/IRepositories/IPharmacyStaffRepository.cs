using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.pharmacystaffs;
using Volo.Abp.Domain.Repositories;

namespace PharmacyAPI.IRepositories
{
    public interface IPharmacyStaffRepository :IRepository<PharmacyStaff,Guid>
    {
      Task<PharmacyStaff> FindUserIdAsync(Guid userId);
      Task<PharmacyStaff> FindByTcKimlikNoAsync(string tcKimlikNo);
      //yoruldum amk
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmacyAPI.EntityFrameworkCore;
using PharmacyAPI.IRepositories;
using PharmacyAPI.pharmacystaffs;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.OpenIddict;

namespace PharmacyAPI
{
    public class PharmacyStaffRepository : EfCoreRepository<PharmacyAPIDbContext, PharmacyStaff, Guid>, IPharmacyStaffRepository
    {
        public PharmacyStaffRepository(IDbContextProvider<PharmacyAPIDbContext> provider) : base(provider) { }

        public async Task<PharmacyStaff> FindByTcKimlikNoAsync(string tcKimlikNo)
        {
            var DbSet = await GetDbSetAsync();
            return await DbSet.FirstOrDefaultAsync(s => s.TcKimlikNo == tcKimlikNo);
        }

        public async Task<PharmacyStaff> FindUserIdAsync(Guid userId)
        {
            var DbSet = await GetDbSetAsync();
            return await DbSet.FirstOrDefaultAsync(s => s.UserId == userId);

        }
    }
}
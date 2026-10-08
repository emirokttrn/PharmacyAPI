using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.Brands;
using Volo.Abp.Domain.Repositories;

namespace PharmacyAPI.IRepositories
{
    public interface IBrandRepository : IRepository<Brand, Guid>
    {
        Task<Brand?> FindByNameAsync(string Name);
    }
}
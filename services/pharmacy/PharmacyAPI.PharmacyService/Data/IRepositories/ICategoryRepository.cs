using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.Categories;
using Volo.Abp.Domain.Repositories;

namespace PharmacyAPI.IRepositories
{
    public interface ICategoryRepository :IRepository<Category,Guid>
    {
        Task<Category?> FindByNameAsync(string name);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.Products;
using Volo.Abp.Domain.Repositories;

namespace PharmacyAPI.IRepositories
{
    public interface IProductRepository :IRepository<Product,Guid>
    {
        Task<Product?> FindByNameAsync(string name);
    }

}
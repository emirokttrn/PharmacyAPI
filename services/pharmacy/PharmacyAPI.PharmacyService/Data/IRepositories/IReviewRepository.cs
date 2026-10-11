using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyAPI.Reviews;
using Volo.Abp.Domain.Repositories;

namespace PharmacyAPI.IRepositories
{
    public interface IReviewRepository : IRepository<Review, Guid>
    {
        Task<Review?> FindByProductAndCustomerAsync(Guid productId, Guid customerId);
        Task<List<Review>> GetListByProductAsync(Guid productId);
    }
}

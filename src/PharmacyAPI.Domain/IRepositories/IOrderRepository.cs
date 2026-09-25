using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyAPI.Orders;
using Volo.Abp.Domain.Repositories;

namespace PharmacyAPI.IRepositories
{
    // [Claude Agent] - Order icin repository, OrderLine'lar icin ayri repository yok (aggregate parcasi)
    public interface IOrderRepository : IRepository<Order, Guid>
    {
        Task<Order?> GetWithDetailsAsync(Guid id);
        Task<List<Order>> GetListByCustomerAsync(Guid customerId);
    }
}

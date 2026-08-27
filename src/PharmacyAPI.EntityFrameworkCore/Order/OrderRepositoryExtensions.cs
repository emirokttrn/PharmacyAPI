using System.Linq;
using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Orders;

namespace PharmacyAPI.Orders
{
    public static class OrderRepositoryExtensions
    {
        public static IQueryable<Order> IncludeDetails(this IQueryable<Order> queryable, bool include = true)
        {
            if (!include) return queryable;
            return queryable.Include(o => o.OrderLines);
        }
    }
}

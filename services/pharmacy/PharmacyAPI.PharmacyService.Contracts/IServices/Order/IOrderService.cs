using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyAPI.OrderDtos;
using Volo.Abp.Application.Services;

namespace PharmacyAPI.IServices.Order
{
    public interface IOrderService : IApplicationService
    {
        Task<OrderDto> CreateAsync(CreateOrderDto request);
        Task<OrderDto> GetAsync(Guid id);
        Task<List<OrderDto>> GetListByCustomerAsync(Guid customerId);
        Task<OrderDto> UpdateStatusAsync(Guid id, UpdateOrderStatusDto request);
    }
}

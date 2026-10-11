using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyAPI.DomainServices;
using PharmacyAPI.IRepositories;
using PharmacyAPI.IServices.Order;
using PharmacyAPI.OrderDtos;
using PharmacyAPI.Orders;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace PharmacyAPI.OrderServices
{
    public class OrderService : ApplicationService, IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly OrderManager _orderManager;

        public OrderService(IOrderRepository repository, OrderManager orderManager)
        {
            _repository = repository;
            _orderManager = orderManager;
        }

        public async Task<OrderDto> CreateAsync(CreateOrderDto request)
        {
            var lines = request.Lines
                .Select(l => new OrderLineRequest(l.ProductId, l.Quantity, l.PrescriptionId))
                .ToList();

            var order = await _orderManager.CreateAsync(request.CustomerId, request.ShippingAddressId, lines);

            await _repository.InsertAsync(order);

            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        public async Task<OrderDto> GetAsync(Guid id)
        {
            var order = await _repository.GetWithDetailsAsync(id);
            if (order == null)
            {
                throw new UserFriendlyException("boyle bir siparis yok!");
            }
            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        public async Task<List<OrderDto>> GetListByCustomerAsync(Guid customerId)
        {
            var orders = await _repository.GetListByCustomerAsync(customerId);
            return ObjectMapper.Map<List<Order>, List<OrderDto>>(orders);
        }

        // [Claude Agent] - Personel siparis durumunu Pending->Confirmed->Shipped->Delivered olarak gunceller
        public async Task<OrderDto> UpdateStatusAsync(Guid id, UpdateOrderStatusDto request)
        {
            var order = await _repository.GetAsync(id);

            order.SetStatus(request.Status);
            if (!string.IsNullOrWhiteSpace(request.TrackingNumber))
            {
                order.SetTrackingNumber(request.TrackingNumber);
            }

            await _repository.UpdateAsync(order);

            return ObjectMapper.Map<Order, OrderDto>(order);
        }
    }
}

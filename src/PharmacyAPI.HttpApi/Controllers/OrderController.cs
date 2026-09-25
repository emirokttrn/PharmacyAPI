using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.IServices.Order;
using PharmacyAPI.OrderDtos;

namespace PharmacyAPI.Controllers
{
    [Route("api/app/order")]
    public class OrderController : PharmacyAPIController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateOrderDto request)
        {
            var result = await _orderService.CreateAsync(request);
            return CreatedAtAction(nameof(GetAsync), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var result = await _orderService.GetAsync(id);
            return Ok(result);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetListByCustomerAsync(Guid customerId)
        {
            var result = await _orderService.GetListByCustomerAsync(customerId);
            return Ok(result);
        }

        // [Claude Agent] - Personel siparis durumunu Pending -> Confirmed -> Shipped -> Delivered olarak gunceller
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatusAsync(Guid id, UpdateOrderStatusDto request)
        {
            var result = await _orderService.UpdateStatusAsync(id, request);
            return Ok(result);
        }
    }
}

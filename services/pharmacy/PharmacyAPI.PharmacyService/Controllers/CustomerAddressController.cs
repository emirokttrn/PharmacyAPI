using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.CustomerAddressDtos;
using PharmacyAPI.IServices.CustomerAddress;

namespace PharmacyAPI.Controllers
{
    [Route("api/app/customer-address")]
    public class CustomerAddressController : PharmacyAPIController
    {
        private readonly ICustomerAddressService _customerAddressService;

        public CustomerAddressController(ICustomerAddressService customerAddressService)
        {
            _customerAddressService = customerAddressService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateCustomerAddressDto request)
        {
            var result = await _customerAddressService.CreateAsync(request);
            return CreatedAtAction(nameof(GetAsync), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var result = await _customerAddressService.GetAsync(id);
            return Ok(result);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetListByCustomerAsync(Guid customerId)
        {
            var result = await _customerAddressService.GetListByCustomerAsync(customerId);
            return Ok(result);
        }

        [HttpPut("{id}/default")]
        public async Task<IActionResult> SetDefaultAsync(Guid id)
        {
            var result = await _customerAddressService.SetDefaultAsync(id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _customerAddressService.DeleteAsync(id);
            return NoContent();
        }
    }
}

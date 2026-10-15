using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.CustomerService.Contracts.Dtos.CustomerAddresses;

namespace PharmacyAPI.CustomerService.Controllers
{
    [Route("api/customer-address")]
    public class CustomerAddressController : CustomerServiceControllerBase
    {
        private readonly ICustomerAddressAppService _customerAddressAppService;

        public CustomerAddressController(ICustomerAddressAppService customerAddressAppService)
        {
            _customerAddressAppService = customerAddressAppService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateCustomerAddressDto input)
        {
            var result = await _customerAddressAppService.CreateCustomerAddressAsync(input);
            return CreatedAtAction(nameof(GetAsync), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var result = await _customerAddressAppService.GetCustomerAdressAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync()
        {
            var result = await _customerAddressAppService.GetAllCustomerAddressesAsync();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, CreateCustomerAddressDto input)
        {
            var result = await _customerAddressAppService.UpdateCustomerAddressAsync(id, input);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _customerAddressAppService.DeleteCustomerAddressAsync(id);
            return NoContent();
        }
    }
}

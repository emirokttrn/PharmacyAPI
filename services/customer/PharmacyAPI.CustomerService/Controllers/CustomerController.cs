using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.CustomerService.Contracts.Dtos.Customers;

namespace PharmacyAPI.CustomerService.Controllers
{
    [Route("api/customer")]
    public class CustomerController : CustomerServiceControllerBase
    {
        private readonly ICustomerAppService _customerAppService;

        public CustomerController(ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateCustomerDto input)
        {
            var result = await _customerAppService.CreateCustomerAsync(input);
            return CreatedAtAction(nameof(GetAsync), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var result = await _customerAppService.GetCustomerAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync()
        {
            var result = await _customerAppService.GetAllCustomersAsync();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateCustomerDto input)
        {
            var result = await _customerAppService.UpdateCustomerAsync(id, input);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _customerAppService.DeleteCustomerAsync(id);
            return NoContent();
        }

        // KVKK: tam (maskesiz) TC Kimlik No sadece bu uc noktada ve sadece admin rolune doner.
        [HttpGet("{id}/tc-kimlik-no")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetTcKimlikNoAsync(Guid id)
        {
            var result = await _customerAppService.GetCustomerTcKimlikNoAsync(id);
            return Ok(result);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.CustomerService.Contracts.IServices;

namespace PharmacyAPI.CustomerService.Controllers
{
    [Route("api/customer-integration")]
    public class CustomerIntegrationController : CustomerServiceControllerBase
    {
        private readonly ICustomerIntegrationService _customerIntegrationService;

        public CustomerIntegrationController(ICustomerIntegrationService customerIntegrationService)
        {
            _customerIntegrationService = customerIntegrationService;
        }

        [HttpGet("order-context")]
        public async Task<IActionResult> GetOrderContextAsync([FromQuery] Guid customerId, [FromQuery] Guid shippingAddressId)
        {
            var result = await _customerIntegrationService.GetOrderContextAsync(customerId, shippingAddressId);
            return Ok(result);
        }
    }
}

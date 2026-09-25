using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.customerdtos;
using PharmacyAPI.IServices.customer;

namespace PharmacyAPI.Controllers
{
    [Route("api/app/customer")]
    public class CustomerController : PharmacyAPIController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateCustomerDto request)
        {
            var result = await _customerService.CreateAsync(request);
            return CreatedAtAction(nameof(GetAsync), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var result = await _customerService.GetAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> FilterAsync([FromQuery] FilterCustomerDto filter)
        {
            var result = await _customerService.FilterAsync(filter);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _customerService.DeleteAsync(id);
            return NoContent();
        }

        // [Claude Agent] - KVKK: tam TC Kimlik No sadece bu ayri endpoint'te donuyor.
        // Not: yetkilendirme (authorization) bu is paketinin kapsami disinda, once eklenmeli.
        [HttpGet("{id}/tc-kimlik-no")]
        public async Task<IActionResult> GetTcKimlikNoAsync(Guid id)
        {
            var result = await _customerService.GetTcKimlikNoAsync(id);
            return Ok(result);
        }
    }
}
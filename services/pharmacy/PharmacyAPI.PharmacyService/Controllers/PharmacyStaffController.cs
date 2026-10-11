using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.IServices.PharmacyStaff;
using PharmacyAPI.pharmacyStaffDTO;

namespace PharmacyAPI.Controllers
{
     [Route("api/app/pharmacy-staff")]
    public class PharmacyStaffController : PharmacyAPIController
    {
        private readonly IPharmacyStaffService _staffService;
 
        public PharmacyStaffController(IPharmacyStaffService staffService)
        {
            _staffService = staffService;
        }
 
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreatePharmacyStaffDto request)
        {
            var result = await _staffService.CreateAsync(request);
            return CreatedAtAction(nameof(GetAsync), new { id = result.Id }, result);
        }
 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var result = await _staffService.GetAsync(id);
            return Ok(result);
        }
 
        [HttpGet]
        public async Task<IActionResult> GetListAsync()
        {
            var result = await _staffService.GetListAsync();
            return Ok(result);
        }
 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _staffService.DeleteAsync(id);
            return NoContent();
        }

        // [Claude Agent] - Not: yetkilendirme (authorization) bu is paketinin kapsami disinda, once eklenmeli.
        [HttpGet("{id}/tc-kimlik-no")]
        public async Task<IActionResult> GetTcKimlikNoAsync(Guid id)
        {
            var result = await _staffService.GetTcKimlikNoAsync(id);
            return Ok(result);
        }
    }
}
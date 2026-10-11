using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.IServices.Prescription;
using PharmacyAPI.PrescriptionDtos;

namespace PharmacyAPI.Controllers
{
    [Route("api/app/prescription")]
    public class PrescriptionController : PharmacyAPIController
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreatePrescriptionDto request)
        {
            var result = await _prescriptionService.CreateAsync(request);
            return CreatedAtAction(nameof(GetAsync), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var result = await _prescriptionService.GetAsync(id);
            return Ok(result);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetListByCustomerAsync(Guid customerId)
        {
            var result = await _prescriptionService.GetListByCustomerAsync(customerId);
            return Ok(result);
        }

        // [Claude Agent] - Eczaci personel receteyi onaylar (sadece Pending durumundan)
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveAsync(Guid id, ReviewPrescriptionDto request)
        {
            var result = await _prescriptionService.ApproveAsync(id, request);
            return Ok(result);
        }

        // [Claude Agent] - Eczaci personel receteyi reddeder (sadece Pending durumundan)
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectAsync(Guid id, ReviewPrescriptionDto request)
        {
            var result = await _prescriptionService.RejectAsync(id, request);
            return Ok(result);
        }
    }
}

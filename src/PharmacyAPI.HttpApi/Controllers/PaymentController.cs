using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.IServices;
using PharmacyAPI.PaymentDtos;

using Route = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace PharmacyAPI.Controllers
{
   [Route("api/app/payment")]
    public class PaymentController : PharmacyAPIController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreatePaymentDto request)
        {
            var result = await _paymentService.CreateAsync(request);
            return CreatedAtAction(nameof(GetAsync), new { id = result.PaymentId }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var result = await _paymentService.GetAsync(id);
            return Ok(result);
        }

        // KRITIK: bu endpoint saglayicinin kendi sunucusundan cagrilir, mobil app'ten DEGIL.
        // [AllowAnonymous] cunku saglayicinin bizim auth token'imiz yok - ama imza
        // dogrulamasi olmadan bu endpoint acik bir kapi olurdu.
        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> HandleWebhookAsync([FromBody] PaymentWebhookDto payload)
        {
            Request.Body.Position = 0;
            using var reader = new StreamReader(Request.Body);
            var rawBody = await reader.ReadToEndAsync();

            var signature = Request.Headers["X-Signature"].ToString();

            // Artik IPaymentProviderService'i controller DIREKT tanimiyor -
            // dogrulama IPaymentService uzerinden, Application katmani araciligiyla yapiliyor
            if (!await _paymentService.VerifyWebhookSignatureAsync(rawBody, signature))
            {
                return Unauthorized();
            }

            await _paymentService.HandleWebhookAsync(payload);
            return Ok();
        }

        [HttpPut("{id}/refund")]
        public async Task<IActionResult> RefundAsync(Guid id, [FromBody] RefundPaymentDto request)
        {
            var result = await _paymentService.RefundAsync(id, request.Reason);
            return Ok(result);
        }
    }
}
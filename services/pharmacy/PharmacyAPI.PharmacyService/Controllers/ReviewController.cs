using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.IServices.Review;
using PharmacyAPI.ReviewDtos;

namespace PharmacyAPI.Controllers
{
    [Route("api/app/review")]
    public class ReviewController : PharmacyAPIController
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateReviewDto request)
        {
            var result = await _reviewService.CreateAsync(request);
            return Created(string.Empty, result);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetListByProductAsync(Guid productId)
        {
            var result = await _reviewService.GetListByProductAsync(productId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _reviewService.DeleteAsync(id);
            return NoContent();
        }
    }
}

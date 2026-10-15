using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace PharmacyAPI.StoreService.Controllers
{
    [Route("api/store")]
    public class StoreReviewController : StoreServiceControllerBase
    {
        private readonly IStoreReviewAppService _storeReviewAppService;

        public StoreReviewController(IStoreReviewAppService storeReviewAppService)
        {
            _storeReviewAppService = storeReviewAppService;
        }

        [HttpGet("{storeId:guid}/reviews")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListAsync(Guid storeId, [FromQuery] PagedResultRequestDto input)
        {
            var result = await _storeReviewAppService.GetListAsync(storeId, input);
            return Ok(result);
        }

        [HttpPost("{storeId:guid}/reviews")]
        [Authorize]
        public async Task<IActionResult> CreateAsync(Guid storeId, CreateStoreReviewDto input)
        {
            var result = await _storeReviewAppService.CreateAsync(storeId, input);
            return Ok(result);
        }

        [HttpPut("reviews/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateStoreReviewDto input)
        {
            var result = await _storeReviewAppService.UpdateAsync(id, input);
            return Ok(result);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PharmacyAPI.StoreService.Controllers
{
    [Route("api/store")]
    public class StoreProductController : StoreServiceControllerBase
    {
        private readonly IStoreProductAppService _storeProductAppService;

        public StoreProductController(IStoreProductAppService storeProductAppService)
        {
            _storeProductAppService = storeProductAppService;
        }

        [HttpGet("products/{productId:guid}/offers")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOffersAsync(Guid productId, [FromQuery] GetStoreOffersInput input)
        {
            var result = await _storeProductAppService.GetOffersAsync(productId, input);
            return Ok(result);
        }

        [HttpGet("{storeId:guid}/products")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListByStoreAsync(Guid storeId)
        {
            var result = await _storeProductAppService.GetListByStoreAsync(storeId);
            return Ok(result);
        }

        [HttpPost("{storeId:guid}/products")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateAsync(Guid storeId, CreateStoreProductDto input)
        {
            var result = await _storeProductAppService.CreateAsync(storeId, input);
            return Ok(result);
        }

        [HttpPut("products/{id:guid}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateStoreProductDto input)
        {
            var result = await _storeProductAppService.UpdateAsync(id, input);
            return Ok(result);
        }

        [HttpDelete("products/{id:guid}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _storeProductAppService.DeleteAsync(id);
            return NoContent();
        }
    }
}

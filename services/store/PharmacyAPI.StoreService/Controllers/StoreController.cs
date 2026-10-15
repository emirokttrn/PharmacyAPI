using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PharmacyAPI.StoreService.Controllers
{
    // {id:guid} kisiti sayesinde "api/store/nearby" ile "api/store/{id}" cakismaz.
    [Route("api/store")]
    public class StoreController : StoreServiceControllerBase
    {
        private readonly IStoreAppService _storeAppService;

        public StoreController(IStoreAppService storeAppService)
        {
            _storeAppService = storeAppService;
        }

        [HttpGet("nearby")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNearbyAsync([FromQuery] GetNearbyStoresInput input)
        {
            var result = await _storeAppService.GetNearbyAsync(input);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var result = await _storeAppService.GetAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateAsync(CreateStoreDto input)
        {
            var result = await _storeAppService.CreateAsync(input);
            return CreatedAtAction(nameof(GetAsync), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateStoreDto input)
        {
            var result = await _storeAppService.UpdateAsync(id, input);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _storeAppService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id:guid}/working-hours")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> SetWorkingHourAsync(Guid id, SetStoreWorkingHoursDto input)
        {
            var result = await _storeAppService.SetWorkingHourAsync(id, input);
            return Ok(result);
        }

        [HttpDelete("{id:guid}/working-hours/{day}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RemoveWorkingHourAsync(Guid id, DayOfWeek day)
        {
            var result = await _storeAppService.RemoveWorkingHourAsync(id, day);
            return Ok(result);
        }

        [HttpPut("{id:guid}/on-duty")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> SetOnDutyAsync(Guid id, [FromQuery] bool isOnDuty)
        {
            var result = await _storeAppService.SetOnDutyAsync(id, isOnDuty);
            return Ok(result);
        }

        [HttpPut("{id:guid}/active")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> SetActiveAsync(Guid id, [FromQuery] bool isActive)
        {
            var result = await _storeAppService.SetActiveAsync(id, isActive);
            return Ok(result);
        }
    }
}

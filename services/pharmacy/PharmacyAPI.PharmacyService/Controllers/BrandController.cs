using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.BranDtos;
using PharmacyAPI.IServices.Brand;
using Volo.Abp.Application.Dtos;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace PharmacyAPI.Controllers
{
    [Route("api/app/brand")]
    public class BrandController : PharmacyAPIController
    {

        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateBrandDto request)
        {
            var result = await _brandService.CreateAsync(request);
            return CreatedAtAction(nameof(GetAsync), new { id = result.Id },result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var result = await _brandService.GetAsync(id);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetListAsync([FromQuery] FilterBrandDto request)
        {
            var result = await _brandService.GetListAsync(request);
            return Ok(result);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
         _brandService.DeleteAsync(id);
         return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, CreateBrandDto request)
        {
            var result = _brandService.UpdateAsync(id,request);
            return Ok(result);
        }

        // [Claude Agent] - Eksik [HttpDelete] attribute'u Swashbuckle'in "Ambiguous HTTP method"
        // hatasiyla TUM swagger.json uretimini kirip /swagger/v1/swagger.json'u 500 dondurmesine
        // sebep oluyordu; ayrica "{id}" ile ayni pattern'e denk gelmesin diye ayri bir alt route verdim.
        [HttpDelete("by-name/{name}")]
        public async Task<IActionResult> DeleteByName(string name)
        {
           await _brandService.DeleteByName(name);
           return NoContent();
        }
    }
}
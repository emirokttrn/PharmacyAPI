using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.BranDtos;
using PharmacyAPI.IServices.Brand;
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
        public async Task DeleteAsync(Guid id)
        {
         _brandService.DeleteAsync(id);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, CreateBrandDto request)
        {
            var result = _brandService.UpdateAsync(id,request);
            return Ok(result);
        }
    }
}
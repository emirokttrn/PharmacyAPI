using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.CategoryDtos;
using PharmacyAPI.IServices.Category;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace PharmacyAPI.Controllers
{
    [Route("api/app/category")]
    public class CategroyController :PharmacyAPIController
    {
        private readonly IcategoryService _service;
        public CategroyController(IcategoryService service)
        {
            _service=service;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateCategoryDto request)
        {
            var result = await _service.CreateCategory(request);
            return CreatedAtAction(nameof(GetAsync), new {id=request.guid}, result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var result =await _service.GetAsync(id);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetListAsync(  )
        {
            var result = await _service.GetListAsync();
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await _service.DeleteAsync(id);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, CreateCategoryDto request)
        {
            var result = await _service.UpdateCategroy(id, request);
            return Ok(result);
        }
    }
}
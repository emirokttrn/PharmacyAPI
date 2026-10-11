using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.ProductDtos;
using PharmacyAPI.IServices.Product;
namespace PharmacyAPI.Controllers
{
    [Route("api/app/product")]
    
    public class ProductController : PharmacyAPIController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // POST /api/app/product
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateProductDto request)
        {
            var result = await _productService.CreateAsync(request);
            return CreatedAtAction(nameof(GetAsync), new { id = result.Id }, result);
        }

        // GET /api/app/product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var result = await _productService.GetAsync(id);
            return Ok(result);
        }

        // GET /api/app/product?filtre-parametreleri
        [HttpGet]
        public async Task<IActionResult> GetListAsync([FromQuery] ProductListFilterDto request)
        {
            var result = await _productService.GetlistAsync(request);
            return Ok(result);
        }

        // PUT /api/app/product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, CreateProductDto request)
        {
            var result = await _productService.UpdateAsync(id, request);
            return Ok(result);
        }

        // DELETE /api/app/product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }

        // GET /api/app/product/best-sellers?count=10
        [HttpGet("best-sellers")]
        public async Task<IActionResult> GetBestSellerProductsAsync([FromQuery] int count)
        {
            var result = await _productService.GetBestSellerProductsAsync(count);
            return Ok(result);
        }

        // GET /api/app/product/new?count=10
        [HttpGet("new")]
        public async Task<IActionResult> GetNewProductsAsync([FromQuery] int count)
        {
            var result = await _productService.GetNewProductsAsync(count);
            return Ok(result);
        }

        // GET /api/app/product/discounted?count=10
        [HttpGet("discounted")]
        public async Task<IActionResult> GetDiscountedProductsAsync([FromQuery] int count)
        {
            var result = await _productService.GetDiscountedProductsAsync(count);
            return Ok(result);
        }

        // GET /api/app/product/category/{id}?filtre-parametreleri
        [HttpGet("category/{id}")]
        public async Task<IActionResult> GetListByCategoryAsync(
            Guid id, [FromQuery] ProductListFilterDto request)
        {
            var result = await _productService.GetListByCategoryAsync(id, request);
            return Ok(result);
        }

        // DELETE /api/app/product/delete-many
        // Not: DELETE metodunda govde (body) kullanmak REST konvansiyonunda tartismali,
        // ama coklu id gondermek icin en pratik yol; ASP.NET Core buna izin veriyor.
        [HttpDelete("delete-many")]
        public async Task<IActionResult> DeleteManyAsync([FromBody] List<Guid> ids)
        {
            await _productService.DeleteManyAsync(ids);
            return NoContent();
        }

        // PUT /api/app/product/{id}/stock-status?inStock=true
        [HttpPut("{id}/stock-status")]
        public async Task<IActionResult> SetStockStatusAsync(Guid id, [FromQuery] bool inStock)
        {
            await _productService.SetStockStatusAsync(id, inStock);
            return NoContent();
        }

        // PUT /api/app/product/stock-status/change-many
        public class ChangeManyStockStatusRequest
        {
            public List<Guid> Ids { get; set; }
            public bool InStock { get; set; }
        }

        [HttpPut("stock-status/change-many")]
        public async Task<IActionResult> ChangeManyStockStatusAsync(
            [FromBody] ChangeManyStockStatusRequest request)
        {
            await _productService.ChangeManyStockStatus(request.Ids, request.InStock);
            return NoContent();
        }
    }
}
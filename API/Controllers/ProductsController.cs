using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GeAllProductsAsync()
        {            
            return Ok(await _productService.GetAllProductsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductByIdAsync(int id)
        {
            return Ok(await _productService.GetProductByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProductAsync([FromBody] CreateProductDto productCreateDto)
        {
            var createResult = await _productService.CreateProductAsync(productCreateDto);            
            return CreatedAtAction(nameof(GetProductByIdAsync), new { id = createResult.Id }, createResult);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProductAsync(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            if (updateProductDto == null || id != updateProductDto.Id)
                return BadRequest();

            var updatedProduct = await _productService.UpdateProductAsync(id, updateProductDto);
            if (updatedProduct == null)
                return NotFound();

            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
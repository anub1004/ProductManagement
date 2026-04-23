using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.DTOs;
using ProductManagement.Models;
using ProductManagement.Services.Implementation;
using ProductManagement.Services.Interfaces;
using System.Globalization;

namespace ProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReaddto>>> GetProducts()
        {
            var result= await _service.GetAllProductsAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReaddto?>> Getoneproduct(string id)
        {
            var result = await _service.GetProductByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromBody] ProductCreatedto productDto)
        {
            var id = await _service.CreateProductAsync(productDto);
            var created = new ProductReaddto
            {
                Id = id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                CreatedAt = DateTime.UtcNow
            };
            return CreatedAtAction(nameof(Getoneproduct), new { id }, created);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(string id, [FromBody] ProductUpdatedto productDto)
        {
            try
            {
                await _service.UpdateProduct(id, productDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound();
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            try
            {
                await _service.DeleteProductAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound();
                return BadRequest(ex.Message);
            }
        }

       


        [HttpPost("uploadcsv")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            try
            {
                var result = await _service.ProcessCsvAsync(file);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
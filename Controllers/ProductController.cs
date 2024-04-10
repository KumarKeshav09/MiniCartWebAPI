using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MintCartWebApi.DBModels;
using MintCartWebApi.ModelDto;
using MintCartWebApi.Service.Product;

namespace MintCartWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("get-product-by-id")]
        public async Task<ActionResult> GetProductById(int productId)
        {
            if (productId > 0)
            {
                var product = await _productService.GetProductByIdAsync(productId);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            else
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get-all-products")]
        public async Task<ActionResult> GetAllProducts(int pageNumber = 1, int pageSize = 10, string search = "")
        {
            if (ModelState.IsValid)
            {
                var data = await _productService.GetAllProductAsync(pageNumber, pageSize, search);
                if (data != null)
                {
                    return Ok(new { success = true, statusCode = 200, data = data });
                }
                else
                {
                    return BadRequest(new { success = false, statusCode = 400, error = 4 });
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                     .Select(e => e.ErrorMessage)
                                     .ToList();

            return BadRequest(new { success = false, statusCode = 400, errors = errors });
        }

        [HttpPost("register-product")]
        public async Task<ActionResult> CreateProduct(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var data = await _productService.CreateProductAsync(productDto);
                if (data != null)
                {
                    return Ok(new { success = true, statusCode = 200, data = data });
                }
                else
                {
                    return BadRequest(new { success = false, statusCode = 400, error = 4 });
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                     .Select(e => e.ErrorMessage)
                                     .ToList();

            return BadRequest(new { success = false, statusCode = 400, errors = errors });
        }


        [HttpPut("update-product")]
        public async Task<ActionResult> UpdateProduct(Product updatedProduct)
        {
            if (updatedProduct != null)
            {
                var data = await _productService.UpdateProductAsync(updatedProduct);
                if (data.Contains("successfully"))
                {
                    return Ok(new { success = true, statusCode = 200, data = data });
                }
                return BadRequest(new { success = false, statusCode = 500, errors = data });
            }
            else
            {
                return BadRequest(new { success = false, statusCode = 500, errors = "Internal server error" });
            }
        }

        [HttpDelete("delete-product")]
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            if (productId > 0)
            {
                var data = await _productService.DeleteProductAsync(productId);
                if (data.Contains("successfully"))
                {
                    return Ok(new { success = true, statusCode = 200, data = data });
                }
                return BadRequest(new { success = false, statusCode = 500, errors = data });
            }
            else
            {
                return BadRequest(new { success = false, statusCode = 500, errors = "Internal server error" });
            }
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{productId}")]
        public async Task<ActionResult> GetProduct(int productId)
        {
            var product = await _productService.GetProductAsync(productId);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(product);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            if (products == null || products.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(products);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct(ProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest();
            }
            else
            {
                var createdProduct = await _productService.CreateProductAsync(productDto);
                return CreatedAtAction(nameof(GetProduct), new { productId = createdProduct.ProductId }, createdProduct);
            }
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult> UpdateProduct(int productId, [FromForm] ProductDto updatedProductDto)
        {
            if (updatedProductDto == null)
            {
                return BadRequest("Updated product data not provided.");
            }

            var updatedProduct = await _productService.UpdateProductAsync(productId, updatedProductDto);

            if (updatedProduct == null)
            {
                return NotFound($"Product with ID {productId} not found.");
            }
            else
            {
                return Ok(updatedProduct);
            }
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            var result = await _productService.DeleteProductAsync(productId);
            if (!result)
            {
                return NotFound();
            }
            else
            {
                return NoContent();
            }
        }
    }
}

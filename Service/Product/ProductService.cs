using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MintCartWebApi.Data;
using MintCartWebApi.ModelDto;
using MintCartWebApi.Utilities;

namespace MintCartWebApi.Service.Product
{

    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(ApplicationDbContext dbContext, IMapper mapper, ILogger<ProductService> logger, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ProductDto> GetProductAsync(int productId)
        {
            _logger.LogInformation("Getting product with ID: {ProductId}", productId);
            try
            {
                var productEntity = await _dbContext.products
                    .Include(p => p.Subcategory)
                    .FirstOrDefaultAsync(p => p.productId == productId);

                if (productEntity == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found", productId);
                    return null;
                }

                var productDto = _mapper.Map<ProductDto>(productEntity);
                productDto.SubcategoryName = productEntity.Subcategory?.subcategoryName;

                return productDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting product with ID: {ProductId}", productId);
                throw;
            }
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            _logger.LogInformation("Getting all products");
            try
            {
                var productEntities = await _dbContext.products.ToListAsync();
                return _mapper.Map<List<ProductDto>>(productEntities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all products");
                throw;
            }
        }

        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            try
            {
                _logger.LogInformation("Creating a new product");

                if (productDto == null)
                {
                    _logger.LogError("Product data not provided.");
                    throw new ArgumentNullException(nameof(productDto), "Product data not provided.");
                }
                var filePath = "/";
                if (productDto.MainProductImage != null && productDto.MainProductImage.Length > 0)
                {
                    filePath = await FileHelper.SaveFileAsync(productDto.MainProductImage, _webHostEnvironment.ContentRootPath);
                }

                if (productDto.ProductImage != null && productDto.ProductImage.Count > 0)
                {
                    var imageUrls = await FileHelper.SaveMultiFilesAsync(productDto.ProductImage, _webHostEnvironment.ContentRootPath);
                    productDto.ProductImageUrl = imageUrls;
                }

                // Retrieve subcategory details from the database
                var subcategory = await _dbContext.Subcategories.FirstOrDefaultAsync(s => s.subcategoryId == productDto.SubcategoryId);
                if (subcategory == null)
                {
                    _logger.LogError($"Subcategory with ID {productDto.SubcategoryId} not found.");
                    throw new ArgumentException($"Subcategory with ID {productDto.SubcategoryId} not found.", nameof(productDto.SubcategoryId));
                }

                var product = _mapper.Map<DBModels.Product>(productDto);
                product.ProductName = productDto.ProductName;
                product.MainProductImageUrl = filePath;
                product.ShortDescription = productDto.ShortDescription;
                product.LongDescription = productDto.LongDescription;
                product.HighlightsOfProduct = productDto.HighlightsOfProduct.ToList();
                product.Specifications = productDto.Specifications;
                product.BrandName = productDto.BrandName;

                product.subcategoryId = subcategory.subcategoryId;

                _dbContext.products.Add(product);
                await _dbContext.SaveChangesAsync();
                var createdProductDto = _mapper.Map<ProductDto>(product);

                _logger.LogInformation("Product created successfully");

                return createdProductDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new product");
                throw;
            }
        }

        public async Task<ProductDto> UpdateProductAsync(int productId, ProductDto updatedProductDto)
        {
            try
            {
                _logger.LogInformation($"Updating product with ID {productId}");
                var existingProduct = await _dbContext.products.FindAsync(productId);
                if (existingProduct == null)
                {
                    _logger.LogError($"Product with ID {productId} not found.");
                    return null;
                }

                existingProduct.ProductName = updatedProductDto.ProductName;
                existingProduct.ShortDescription = updatedProductDto.ShortDescription;
                existingProduct.LongDescription = updatedProductDto.LongDescription;
                existingProduct.HighlightsOfProduct = updatedProductDto.HighlightsOfProduct.ToList();
                existingProduct.Specifications = updatedProductDto.Specifications;
                existingProduct.BrandName = updatedProductDto.BrandName;

                if (updatedProductDto.MainProductImage != null && updatedProductDto.MainProductImage.Length > 0)
                {
                    existingProduct.MainProductImageUrl = await FileHelper.SaveFileAsync(updatedProductDto.MainProductImage, _webHostEnvironment.ContentRootPath);
                }

                if (updatedProductDto.ProductImage != null && updatedProductDto.ProductImage.Count > 0)
                {
                    var imageUrls = await FileHelper.SaveMultiFilesAsync(updatedProductDto.ProductImage, _webHostEnvironment.ContentRootPath);
                    existingProduct.ProductImageUrl = imageUrls;
                }

                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Product with ID {productId} updated successfully");
                return _mapper.Map<ProductDto>(existingProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating product with ID {productId}");
                throw;
            }
        }


        public async Task<bool> DeleteProductAsync(int productId)
        {
            _logger.LogInformation("Deleting product with ID: {ProductId}", productId);
            try
            {
                var productEntity = await _dbContext.products.FindAsync(productId);
                if (productEntity == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found for deletion", productId);
                    return false;
                }

                _dbContext.products.Remove(productEntity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product with ID: {ProductId}", productId);
                throw;
            }
        }
    }
}

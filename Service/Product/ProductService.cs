using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MintCartWebApi.Common;
using MintCartWebApi.Data;
using MintCartWebApi.DBModels;
using MintCartWebApi.LoggerService;
using MintCartWebApi.ModelDto;
using MintCartWebApi.Utilities;
using static System.Net.Mime.MediaTypeNames;

namespace MintCartWebApi.Service.Product
{

    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(ApplicationDbContext dbContext, IMapper mapper, ILoggerManager logger, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<DBModels.Product> GetProductByIdAsync(int productId)
        {
            try
            {
                _logger.LogInfo("Retrieving category by Id: {CategoryId}");
                var productById = await _dbContext.products.Include(p => p.Subcategory).ThenInclude(s => s.Category).FirstOrDefaultAsync(p => p.productId == productId);
                return productById;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving category by Id: {productId} ,{ex.Message}");
                throw;
            }
        }

        public async Task<List<DBModels.Product>> GetAllProductAsync(int pageNumber, int pageSize, string search)
        {
            try
            {
                _logger.LogInfo("Retrieving all categories");
                var productQuery = _dbContext.products.Include(p => p.Subcategory).ThenInclude(s => s.Category).AsQueryable();
                if (!string.IsNullOrEmpty(search))
                {
                    productQuery = productQuery.Where(c => c.ProductName.Contains(search) || c.BrandName.Contains(search));
                }
                var productDetails = await productQuery
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();

                return productDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving all categories {ex.Message}");
                throw;
            }
        }

        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            try
            {
                _logger.LogInfo("Creating a new product");

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

                var subcategory = await _dbContext.Subcategories.Include(s => s.Category).FirstOrDefaultAsync(s => s.subcategoryId == productDto.Subcategory.subcategoryId);
                if (subcategory == null)
                {
                    _logger.LogError($"Sub Category with ID {productDto.Subcategory.subcategoryId} not found.");
                    throw new ArgumentException($"Subcategory with ID {productDto.Subcategory.subcategoryId} not found.", nameof(productDto.Subcategory.subcategoryId));
                }

                var product = _mapper.Map<DBModels.Product>(productDto);
                product.ProductName = productDto.ProductName;
                product.MainProductImageUrl = filePath;
                product.ShortDescription = productDto.ShortDescription;
                product.LongDescription = productDto.LongDescription;
                product.HighlightsOfProduct = productDto.HighlightsOfProduct.ToList();
                product.Specifications = productDto.Specifications;
                product.BrandName = productDto.BrandName;
                product.Subcategory = subcategory;


                _dbContext.products.Add(product);
                await _dbContext.SaveChangesAsync();
                var createdProductDto = _mapper.Map<ProductDto>(product);

                _logger.LogInfo("Product created successfully");

                return createdProductDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving all categories {ex.Message}");
                throw;
            }
        }

        public async Task<string> UpdateProductAsync(DBModels.Product updatedProduct)
        {
            try
            {
                _logger.LogInfo($"Updating product with ID {updatedProduct.productId}");
                if (updatedProduct == null)
                {
                    _logger.LogError("Updated product data not provided.");
                    throw new ArgumentNullException(nameof(updatedProduct), "Updated product data not provided.");
                }
                var existingProduct = await _dbContext.products
                    .Include(p => p.Subcategory)
                    .FirstOrDefaultAsync(p => p.productId == updatedProduct.productId);
                if (existingProduct == null)
                {
                    _logger.LogError($"Product with ID {updatedProduct.productId} not found.");
                    throw new ArgumentException($"Product with ID {updatedProduct.productId} not found.", nameof(updatedProduct));
                }

                existingProduct.ProductName = updatedProduct.ProductName;
                existingProduct.ShortDescription = updatedProduct.ShortDescription;
                existingProduct.LongDescription = updatedProduct.LongDescription;
                existingProduct.HighlightsOfProduct = updatedProduct.HighlightsOfProduct.ToList();
                existingProduct.Specifications = updatedProduct.Specifications;
                existingProduct.BrandName = updatedProduct.BrandName;
                var subcategory = await _dbContext.Subcategories.Include(s => s.Category).FirstOrDefaultAsync(s => s.subcategoryId == updatedProduct.Subcategory.subcategoryId);
                if (subcategory == null)
                {
                    throw new ArgumentException($"Category with ID {updatedProduct.Subcategory.categoryId} not found.");
                }
                existingProduct.Subcategory = subcategory;

                _dbContext.Update(updatedProduct);
                await _dbContext.SaveChangesAsync();


                _logger.LogInfo($"Product with ID {updatedProduct.productId} updated successfully");

                return MessagesAlerts.SuccessfullSave;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product with ID {updatedProduct.productId}: {ex.Message}");
                throw;
            }
        }

        public async Task<string> DeleteProductAsync(int productId)
        {
            string message = MessagesAlerts.FailDelete;
            try
            {
                _logger.LogInfo("Deleting product with ID: {ProductId}");
                var productEntity = await _dbContext.products.FindAsync(productId);
                if (productEntity != null)
                {
                    _dbContext.products.Remove(productEntity);
                    await _dbContext.SaveChangesAsync();
                    message = MessagesAlerts.SuccessfullDelete;
                }
                return message;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting category with Id: {productId} , {ex.Message}");
                throw;
            }
        }
    }
}

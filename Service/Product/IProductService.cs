using MintCartWebApi.ModelDto;

namespace MintCartWebApi.Service.Product
{
    public interface IProductService
    {
        public Task<ProductDto> GetProductAsync(int productId);
        public Task<List<ProductDto>> GetAllProductsAsync();
        public Task<ProductDto> CreateProductAsync(ProductDto productDto);
        public Task<ProductDto> UpdateProductAsync(int productId, ProductDto updatedProductDto);
        public Task<bool> DeleteProductAsync(int productId);
    }
}

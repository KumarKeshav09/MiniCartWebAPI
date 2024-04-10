using MintCartWebApi.ModelDto;

namespace MintCartWebApi.Service.Product
{
    public interface IProductService
    {
        public Task<DBModels.Product> GetProductByIdAsync(int productId);
        public Task<List<DBModels.Product>> GetAllProductAsync(int pageNumber, int pageSize, string search);
        public Task<ProductDto> CreateProductAsync(ProductDto productDto);
        public Task<string> UpdateProductAsync(DBModels.Product updatedProduct);
        public Task<string> DeleteProductAsync(int productId);
    }
}

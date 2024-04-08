using MintCartWebApi.ModelDto;

namespace MintCartWebApi.Service.Category
{
    public interface ICategoryService
    {
        public Task<DBModels.Category> GetCategoryByIdAsync(int categoryId);
        public Task<List<DBModels.Category>> GetAllCategoriesAsync(int pageNumber, int pageSize, string search);
        public Task<DBModels.Category> CreateCategoryAsync(RegisterCategoryDto category);
        public Task<string> UpdateCategoryAsync(DBModels.Category category);
        public Task<string> DeleteCategoryAsync(int categoryId);
    }
}

namespace MintCartWebApi.Service.Category
{
    public interface ICategoryService
    {
        public Task<DBModels.Category> GetCategoryByIdAsync(int categoryId);
        public Task<List<DBModels.Category>> GetAllCategoriesAsync();
        public Task<DBModels.Category> CreateCategoryAsync(DBModels.Category category);
        public Task UpdateCategoryAsync(DBModels.Category category);
        public Task DeleteCategoryAsync(int categoryId);
    }
}

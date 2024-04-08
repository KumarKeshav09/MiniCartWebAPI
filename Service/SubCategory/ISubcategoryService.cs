using MintCartWebApi.DBModels;

namespace MintCartWebApi.Service.SubCategory
{
    public interface ISubcategoryService
    {
        public Task<Subcategory> CreateSubcategoryAsync(Subcategory subcategory, int categoryId);
        public Task<List<Subcategory>> GetAllSubcategoriesAsync();
        public Task<Subcategory> GetSubcategoryByIdAsync(int id);
        public Task UpdateSubcategoryAsync(Subcategory subcategory);
        public Task DeleteSubcategoryAsync(int id);
    }
}

using MintCartWebApi.DBModels;
using MintCartWebApi.ModelDto;

namespace MintCartWebApi.Service.SubCategory
{
    public interface ISubcategoryService
    {
        public Task<Subcategory> CreateSubcategoryAsync(SubCategoryDto subcategory, int categoryId);
        public Task<List<DBModels.Subcategory>> GetAllSubCategoriesAsync(int pageNumber, int pageSize, string search);
        public Task<Subcategory> GetSubcategoryByIdAsync(int id);
        public Task<string> UpdateSubcategoryAsync(Subcategory subcategory);
        public Task<string> DeleteSubcategoryAsync(int id);
    }
}

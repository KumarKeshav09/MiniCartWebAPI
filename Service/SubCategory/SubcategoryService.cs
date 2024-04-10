using Microsoft.EntityFrameworkCore;
using MintCartWebApi.Common;
using MintCartWebApi.Data;
using MintCartWebApi.DBModels;
using MintCartWebApi.LoggerService;
using MintCartWebApi.ModelDto;

namespace MintCartWebApi.Service.SubCategory
{
    public class SubcategoryService : ISubcategoryService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILoggerManager _logger;

        public SubcategoryService(ApplicationDbContext dbContext, ILoggerManager logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Subcategory> CreateSubcategoryAsync(SubCategoryDto subcategory, int categoryId)
        {
            try
            {
                var category = await _dbContext.Categories.FindAsync(categoryId);
                if (category == null)
                {
                    throw new ArgumentException($"Category with ID {categoryId} not found.");
                }
                var newSubCategory = new DBModels.Subcategory();
                newSubCategory.subcategoryName = subcategory.subcategoryName;
                newSubCategory.subcategoryDes = subcategory.subcategoryDes;
                newSubCategory.Category = category;
                await _dbContext.Subcategories.AddAsync(newSubCategory);
                await _dbContext.SaveChangesAsync();

                return newSubCategory;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving category by Id: {categoryId} ,{ex.Message}");
                throw;
            }
        }

        public async Task<List<DBModels.Subcategory>> GetAllSubCategoriesAsync(int pageNumber, int pageSize, string search)
        {
            try
            {
                _logger.LogInfo("Retrieving all sub categories");
                var subcategoriesQuery = _dbContext.Subcategories.Include(s => s.Category).AsQueryable();
                if (!string.IsNullOrEmpty(search))
                {
                    subcategoriesQuery = subcategoriesQuery.Where(c => c.subcategoryName.Contains(search) || c.subcategoryDes.Contains(search));
                }
                var subcategories = await subcategoriesQuery
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();

                return subcategories;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving all categories {ex.Message}");
                throw;
            }
        }

        public async Task<Subcategory> GetSubcategoryByIdAsync(int id)
        {
            try
            {
                return await _dbContext.Subcategories.Include(s => s.Category).FirstOrDefaultAsync(s => s.subcategoryId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving Sub category by Id: {id} ,{ex.Message}");
                throw;
            }
        }

        public async Task<string> UpdateSubcategoryAsync(Subcategory subcategory)
        {
            string message = MessagesAlerts.FailUpdate;
            try
            {
                _logger.LogInfo("Updating subcategory: {@subcategory}");

                // Find the existing subcategory by its ID
                var existingSubcategory = await _dbContext.Subcategories.FindAsync(subcategory.subcategoryId);
                if (existingSubcategory != null)
                {
                    existingSubcategory.subcategoryName = subcategory.subcategoryName;
                    existingSubcategory.subcategoryDes = subcategory.subcategoryDes;

                    var category = await _dbContext.Categories.FindAsync(subcategory.categoryId);
                    if (category == null)
                    {
                        throw new ArgumentException($"Category with ID {subcategory.categoryId} not found.");
                    }

                    existingSubcategory.Category = category;

                    await _dbContext.SaveChangesAsync();

                    message = MessagesAlerts.SuccessfullUpdate;
                }
                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating sub category: {ex.Message}");
                throw;
            }
        }

        public async Task<string> DeleteSubcategoryAsync(int id)
        {
             string message = MessagesAlerts.FailDelete;
            try
            {
                _logger.LogInfo($"Deleting category with Id: {id}");
                var subcategory = await _dbContext.Subcategories.FindAsync(id);
                if (subcategory != null)
                {
                    _dbContext.Subcategories.Remove(subcategory);
                    await _dbContext.SaveChangesAsync();
                    message = MessagesAlerts.SuccessfullDelete;
                }
                return message;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting category with Id: {id} , {ex.Message}");
                throw;
            }
        }
    }
}

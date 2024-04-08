
using Microsoft.EntityFrameworkCore;
using MintCartWebApi.Data;

namespace MintCartWebApi.Service.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ApplicationDbContext dbContext, ILogger<CategoryService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<DBModels.Category> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                _logger.LogInformation("Retrieving category by Id: {CategoryId}", categoryId);
                var category = await _dbContext.Categories.FindAsync(categoryId);
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category by Id: {CategoryId}", categoryId);
                throw;
            }
        }

        public async Task<List<DBModels.Category>> GetAllCategoriesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all categories");
                var categories = await _dbContext.Categories.ToListAsync();
                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all categories");
                throw;
            }
        }

        public async Task<DBModels.Category> CreateCategoryAsync(DBModels.Category category)
        {
            try
            {
                var newCategory = new DBModels.Category
                {
                categoryName = category.categoryName,
                categoryDes = category.categoryDes 
                };
                await _dbContext.Categories.AddAsync(newCategory);   
                await _dbContext.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category: {@Category}", category);
                throw;
            }
        }

        public async Task UpdateCategoryAsync(DBModels.Category category)
        {
            try
            {
                _logger.LogInformation("Updating category: {@Category}", category);
                _dbContext.Entry(category).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category: {@Category}", category);
                throw;
            }
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            try
            {
                _logger.LogInformation("Deleting category with Id: {CategoryId}", categoryId);
                var category = await _dbContext.Categories.FindAsync(categoryId);
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category with Id: {CategoryId}", categoryId);
                throw; // Re-throw the exception to propagate it to the caller
            }
        }
    }
}


using Microsoft.EntityFrameworkCore;
using MintCartWebApi.Common;
using MintCartWebApi.Data;
using MintCartWebApi.LoggerService;
using MintCartWebApi.ModelDto;

namespace MintCartWebApi.Service.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILoggerManager _logger;

        public CategoryService(ApplicationDbContext dbContext, ILoggerManager logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<DBModels.Category> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                _logger.LogInfo("Retrieving category by Id: {CategoryId}");
                var category = await _dbContext.Categories.FindAsync(categoryId);
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving category by Id: {categoryId} ,{ex.Message}");
                throw;
            }
        }

        public async Task<List<DBModels.Category>> GetAllCategoriesAsync(int pageNumber, int pageSize, string search)
        {
            try
            {
                _logger.LogInfo("Retrieving all categories");
                var categoriesQuery = _dbContext.Categories.AsQueryable();
                if (!string.IsNullOrEmpty(search))
                {
                    categoriesQuery = categoriesQuery.Where(c => c.categoryName.Contains(search) || c.categoryDes.Contains(search));
                }
                var categories = await categoriesQuery
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();

                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving all categories {ex.Message}");
                throw;
            }
        }

        public async Task<DBModels.Category> CreateCategoryAsync(RegisterCategoryDto category)
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
                return newCategory;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating category: {ex.Message}");
                throw;
            }
        }

        public async Task<string> UpdateCategoryAsync(DBModels.Category category)
        {
            string message = MessagesAlerts.FailUpdate;
            try
            {
                _logger.LogInfo("Updating category: {@Category}");
             var exCategory =   await _dbContext.Categories.FirstOrDefaultAsync(u => u.categoryId == category.categoryId);  
                if (exCategory != null)
                {
                   exCategory.categoryName = category.categoryName;
                    exCategory.categoryDes = category.categoryDes;
                    _dbContext.Update(exCategory);
                    await _dbContext.SaveChangesAsync();
                    message = MessagesAlerts.SuccessfullUpdate;
                }
                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating category: {ex.Message}");
                throw;
            }
        }

        public async Task<string> DeleteCategoryAsync(int categoryId)
        {
            string message = MessagesAlerts.FailDelete;
            try
            {
                _logger.LogInfo($"Deleting category with Id: {categoryId}");
                var category = await _dbContext.Categories.FindAsync(categoryId);
                if(category != null)
                {
                    _dbContext.Categories.Remove(category);
                    await _dbContext.SaveChangesAsync();
                    message = MessagesAlerts.SuccessfullDelete;
                }
                return message;
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting category with Id: {categoryId} , {ex.Message}");
                throw;
            }
        }
    }
}

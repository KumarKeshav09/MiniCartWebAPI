using Microsoft.EntityFrameworkCore;
using MintCartWebApi.Data;
using MintCartWebApi.DBModels;

namespace MintCartWebApi.Service.SubCategory
{
    public class SubcategoryService : ISubcategoryService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<SubcategoryService> _logger;

        public SubcategoryService(ApplicationDbContext dbContext, ILogger<SubcategoryService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Subcategory> CreateSubcategoryAsync(Subcategory subcategory, int categoryId)
        {
            try
            {
                var category = await _dbContext.Categories.FindAsync(categoryId);
                if (category == null)
                {
                    throw new ArgumentException($"Category with ID {categoryId} not found.");
                }

                subcategory.Category = category; // Associate subcategory with the category
                await _dbContext.Subcategories.AddAsync(subcategory);
                await _dbContext.SaveChangesAsync();

                return subcategory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating subcategory: {@Subcategory}", subcategory);
                throw;
            }
        }

        public async Task<List<Subcategory>> GetAllSubcategoriesAsync()
        {
            try
            {
                return await _dbContext.Subcategories.Include(s => s.Category).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all subcategories");
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
                _logger.LogError(ex, "Error retrieving subcategory by ID: {Id}", id);
                throw;
            }
        }

        public async Task UpdateSubcategoryAsync(Subcategory subcategory)
        {
            try
            {
                _dbContext.Entry(subcategory).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating subcategory: {@Subcategory}", subcategory);
                throw;
            }
        }

        public async Task DeleteSubcategoryAsync(int id)
        {
            try
            {
                var subcategory = await _dbContext.Subcategories.FindAsync(id);
                if (subcategory == null)
                {
                    throw new ArgumentException($"Subcategory with ID {id} not found.");
                }

                _dbContext.Subcategories.Remove(subcategory);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting subcategory with ID: {Id}", id);
                throw;
            }
        }
    }
}

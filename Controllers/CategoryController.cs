using Microsoft.AspNetCore.Mvc;
using MintCartWebApi.DBModels;
using MintCartWebApi.ModelDto;
using MintCartWebApi.Service;
using MintCartWebApi.Service.Category;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MintCartWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<Category>> GetCategoryById(int categoryId)
        {
            if (categoryId > 0)
            {
                var category = await _categoryService.GetCategoryByIdAsync(categoryId);
                if (category == null)
                {
                    return NotFound();
                }
                return Ok(category);
            }
            else
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAllCategories()
        {
            if (ModelState.IsValid)
            {
                var data = await _categoryService.GetAllCategoriesAsync();
                if (data != null)
                {
                    return Ok(new { success = true, statusCode = 200, data = data });
                }
                else
                {
                    return BadRequest(new { success = false, statusCode = 400, error = 4 });
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                     .Select(e => e.ErrorMessage)
                                     .ToList();

            return BadRequest(new { success = false, statusCode = 400, errors = errors });
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory([FromForm]Category category)
        {
            if (ModelState.IsValid)
            {
                var data = await _categoryService.CreateCategoryAsync(category);
                if (data != null)
                {
                    return Ok(new { success = true, statusCode = 200, data = data });
                }
                else
                {
                    return BadRequest(new { success = false, statusCode = 400, error = 4 });
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                     .Select(e => e.ErrorMessage)
                                     .ToList();

            return BadRequest(new { success = false, statusCode = 400, errors = errors });
        }

        [HttpPut("{categoryId}")]
        public async Task<ActionResult> UpdateCategory(int categoryId, Category category)
        {
            if (categoryId == category.categoryId)
            {
                await _categoryService.UpdateCategoryAsync(category);
                return NoContent();
            }
            else
            {
                return BadRequest(new { success = false, statusCode = 500, errors = "Internal server error" });
            }
        }

        [HttpDelete("{categoryId}")]
        public async Task<ActionResult> DeleteCategory(int categoryId)
        {
            if (categoryId > 0)
            {
                await _categoryService.DeleteCategoryAsync(categoryId);
                return NoContent();
            }
            else
            {
                return BadRequest(new { success = false, statusCode = 500, errors = "Internal server error" });
            }
        }
    }
}

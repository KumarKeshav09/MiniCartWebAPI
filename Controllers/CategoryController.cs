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

        [HttpGet("get-category-by-id")]
        public async Task<ActionResult> GetCategoryById(int categoryId)
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

        [HttpGet("get-all-category")]
        public async Task<ActionResult> GetAllCategories(int pageNumber = 1, int pageSize = 10, string search = "")
        {
            if (ModelState.IsValid)
            {
                var data = await _categoryService.GetAllCategoriesAsync(pageNumber, pageSize, search);
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

        [HttpPost("register-category")]
        public async Task<ActionResult> CreateCategory(RegisterCategoryDto model)
        {
            if (ModelState.IsValid)
            {
                var data = await _categoryService.CreateCategoryAsync(model);
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

        [HttpPut("update-category")]
        public async Task<ActionResult> UpdateCategory(Category category)
        {
            if (category != null)
            {
               var data = await _categoryService.UpdateCategoryAsync(category);
                if (data.Contains("successfully"))
                {
                    return Ok(new { success = true, statusCode = 200, data = data });
                }
                return BadRequest(new { success = false, statusCode = 500, errors = data });
            }
            else
            {
                return BadRequest(new { success = false, statusCode = 500, errors = "Internal server error" });
            }
        }

        [HttpDelete("delete-category")]
        public async Task<ActionResult> DeleteCategory(int categoryId)
        {
            if (categoryId > 0)
            {
               var data =  await _categoryService.DeleteCategoryAsync(categoryId);
                if (data.Contains("successfully"))
                {
                    return Ok(new { success = true, statusCode = 200, data = data });
                }
                return BadRequest(new { success = false, statusCode = 500, errors = data });
            }
            else
            {
                return BadRequest(new { success = false, statusCode = 500, errors = "Internal server error" });
            }
        }
    }
}

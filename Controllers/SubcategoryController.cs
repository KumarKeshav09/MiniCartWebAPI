using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MintCartWebApi.DBModels;
using MintCartWebApi.ModelDto;
using MintCartWebApi.Service.SubCategory;

namespace MintCartWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoryController : ControllerBase
    {
        private readonly ISubcategoryService _subcategoryService;

        public SubcategoryController(ISubcategoryService subcategoryService)
        {
            _subcategoryService = subcategoryService;
        }


        [HttpPost("register-SubCategory")]
        public async Task<ActionResult<Subcategory>> CreateSubcategory(int categoryId, SubCategoryDto model)
        {
            if (ModelState.IsValid)
            {
                var data = await _subcategoryService.CreateSubcategoryAsync(model, categoryId);
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

        [HttpGet("get-all-subcategory")]
        public async Task<ActionResult> GetAllSubCategories(int pageNumber = 1, int pageSize = 10, string search = "")
        {
            if (ModelState.IsValid)
            {
                var data = await _subcategoryService.GetAllSubCategoriesAsync(pageNumber, pageSize, search);
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

        [HttpGet("get-subcategory-by-id")]
        public async Task<ActionResult> GetSubcategoryById(int id)
        {
            if (id > 0)
            {
                var subcategory = await _subcategoryService.GetSubcategoryByIdAsync(id);
                if (subcategory == null)
                {
                    return NotFound();
                }
                return Ok(subcategory);
            }
            else
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update-subcategory")]

        public async Task<ActionResult> UpdateSubcategory(Subcategory subcategory)
        {
            if (subcategory != null)
            {
                var data = await _subcategoryService.UpdateSubcategoryAsync(subcategory);
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

        [HttpDelete("delete-subcategory")]
        public async Task<ActionResult> DeleteSubcategory(int id)
        {
            if (id > 0)
            {
                var data = await _subcategoryService.DeleteSubcategoryAsync(id);
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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MintCartWebApi.DBModels;
using MintCartWebApi.Service.SubCategory;

namespace MintCartWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoryController : ControllerBase
    {
        private readonly ISubcategoryService _subcategoryService;
        private readonly ILogger<SubcategoryController> _logger;

        public SubcategoryController(ISubcategoryService subcategoryService, ILogger<SubcategoryController> logger)
        {
            _subcategoryService = subcategoryService;
            _logger = logger;
        }


        [HttpPost("{categoryId}")]
        public async Task<ActionResult<Subcategory>> CreateSubcategory(int categoryId, Subcategory subcategory)
        {
            try
            {
                var createdSubcategory = await _subcategoryService.CreateSubcategoryAsync(subcategory, categoryId);
                return CreatedAtAction(nameof(GetSubcategoryById), new { id = createdSubcategory.subcategoryId }, createdSubcategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating subcategory: {@Subcategory}", subcategory);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        public async Task<ActionResult<List<Subcategory>>> GetAllSubcategories()
        {
            var subcategories = await _subcategoryService.GetAllSubcategoriesAsync();
            if (subcategories == null || subcategories.Count == 0)
            {
                return NoContent(); // Return 204 No Content if no subcategories found
            }
            return Ok(subcategories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Subcategory>> GetSubcategoryById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid subcategory ID");
            }

            var subcategory = await _subcategoryService.GetSubcategoryByIdAsync(id);
            if (subcategory == null)
            {
                return NotFound(); // Return 404 Not Found if subcategory not found
            }
            return Ok(subcategory);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSubcategory(int id, Subcategory subcategory)
        {
            if (id <= 0 || subcategory == null || id != subcategory.subcategoryId)
            {
                return BadRequest("Invalid subcategory data or ID");
            }

            await _subcategoryService.UpdateSubcategoryAsync(subcategory);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubcategory(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid subcategory ID");
            }

            await _subcategoryService.DeleteSubcategoryAsync(id);
            return NoContent();
        }
    }
}

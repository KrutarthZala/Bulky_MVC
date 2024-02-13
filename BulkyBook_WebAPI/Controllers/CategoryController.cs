using BulkyBook_WebAPI.Data;
using BulkyBook_WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Category GET
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _context.Category.ToListAsync();

                // Validate categories
                if (categories == null || !categories.Any())
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "No categories found"
                    });
                }

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = categories
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }
        #endregion

        #region Category GET with Specific CategoryID
        [HttpGet("{CategoryID}")]
        public async Task<IActionResult> GetSpecificCategory(int CategoryID)
        {
            // Validate CategoryID
            if (CategoryID < 1)
            {
                return BadRequest(new { StatusCode = 400, Message = "Bad Request" });
            }

            var category = await _context.Category.FirstOrDefaultAsync(p => p.CategoryID == CategoryID);

            // Validate category
            if (category == null)
            {
                return NotFound(new { StatusCode = 404, Message = "Not Found" });
            }
            return Ok(new { StatusCode = 200, Message = "Success", Data = category });
        }
        #endregion

        #region Category POST / Add Category
        [HttpPost]
        public async Task<IActionResult> PostCategory(CategoryModel category)
        {
            try
            {
                // Validate category
                if (category == null)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Bad Request"
                    });
                }

                _context.Category.Add(category);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Category inserted successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(406, new { Message = "Not Acceptable", Details = ex.Message });
            }
        }
        #endregion

        #region Category PUT / Update Category
        [HttpPut]
        public async Task<IActionResult> PutProduct(CategoryModel category)
        {
            // Validate CategoryID and category
            if (category == null || category.CategoryID == 0)
            {
                return BadRequest(new { StatusCode = 400, Message = "Bad Request" });
            }

            var newCategory = await _context.Category.FindAsync(category.CategoryID);

            // Validate newCategory
            if (newCategory == null)
            {
                return NotFound(new { StatusCode = 404, Message = "Not Found" });
            }

            newCategory.CategoryName = category.CategoryName;
            newCategory.CategoryOrder = category.CategoryOrder;

            await _context.SaveChangesAsync();
            return Ok(new { StatusCode = 200, Message = "Category Updated successfully" });
        }
        #endregion

        #region Category DELETE
        [HttpDelete("{CategoryID}")]
        public async Task<IActionResult> DeleteProduct(int CategoryID)
        {
            // Validate CategoryID
            if (CategoryID < 1)
            {
                return BadRequest(new { StatusCode = 400, Message = "Bad Request" });
            }

            var category = await _context.Category.FindAsync(CategoryID);

            // Validate category
            if (category == null)
            {
                return NotFound(new { StatusCode = 404, Message = "Not Found" });
            }
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return Ok(new { StatusCode = 200, Message = "Category deleted successfully" });
        }
        #endregion
    }
}

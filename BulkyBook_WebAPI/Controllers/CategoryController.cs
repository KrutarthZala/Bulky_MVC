using BulkyBook_WebAPI.Models;
using BulkyBook_WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        //private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;

        public CategoryController(/*IUnitOfWork unitOfWork*/ICategoryService categoryService)
        {
            _categoryService = categoryService;
            //_unitOfWork = unitOfWork;
        }

        #region Category GET
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                //var categories = _unitOfWork.Category.GetAll();
                var categories = await _categoryService.GetCategories();

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

            //var category = _unitOfWork.Category.Get(p => p.CategoryID == CategoryID);
            var category = await  _categoryService.GetCategory(CategoryID);

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

                //await _unitOfWork.Category.Add(category);
                //await _unitOfWork.Save();

                await _categoryService.InsertCategory(category);
                await _categoryService.SaveCategory();
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
        public async Task<IActionResult> PutCategory(CategoryModel category)
        {
            // Validate CategoryID and category
            if (category == null || category.CategoryID == 0)
            {
                return BadRequest(new { StatusCode = 400, Message = "Bad Request" });
            }

            //newCategory.CategoryName = category.CategoryName;
            //newCategory.CategoryOrder = category.CategoryOrder;

            // await _unitOfWork.Save();
            await _categoryService.UpdateCategory(category);
            await _categoryService.SaveCategory();
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

            var category = await _categoryService.GetCategory(CategoryID);

            // Validate category
            if (category == null)
            {
                return NotFound(new { StatusCode = 404, Message = "Not Found" });
            }
            //await _unitOfWork.Category.Remove(category);
            //await _unitOfWork.Save();
            await _categoryService.DeleteCategory(category);
            await _categoryService.SaveCategory();
            return Ok(new { StatusCode = 200, Message = "Category deleted successfully" });
        }
        #endregion
    }
}

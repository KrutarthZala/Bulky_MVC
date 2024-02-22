using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    // Define Area and Authorize role.
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        // Instance of Dependency Injection
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Display Category
        public IActionResult Index()
        {
            // Retrieve list of Category
            List<CategoryModel> categoryList = _unitOfWork.Category.GetAll().ToList();
            return View(categoryList);
        }
        #endregion

        #region Create Category
    
        // Handle GET request
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryModel category)
        {
            // If model is bound then create a category 
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        #endregion

        #region Edit Category
        public IActionResult EditCategory(int? categoryID)
        {
            // CategoryID Validation
            if (categoryID == null || categoryID < 0)
            {
                return NotFound();
            }

            // Use Repository Get method
            CategoryModel? categoryFromDb = _unitOfWork.Category.Get(u => u.CategoryID == categoryID);
            
            // Example of FirstOrDefault Method
            // CategoryModel? categoryFromDb1 = _dbCategory.Categories.FirstOrDefault(u=>u.CategoryID == categoryID);

            // Example of FirstOrDefault with Where Clause
            // CategoryModel? categoryFromDb2 = _dbCategory.Categories.Where(u => u.CategoryID == categoryID).FirstOrDefault();

            // Check if data is found or not
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryModel categoryObj)
        {
            // If model is bound then edit a category
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(categoryObj);
                _unitOfWork.Save();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        #endregion

        #region Delete Category
        public IActionResult DeleteCategory(int? categoryID)
        {
            // Check CategoryID Validations
            if (categoryID == null || categoryID < 0)
            {
                return NotFound();
            }

            // Retrieve Category using Get Method
            CategoryModel? categoryFromDb = _unitOfWork.Category.Get(u => u.CategoryID == categoryID);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        // Define custom ActionName to maintain naming in View
        [HttpPost, ActionName("DeleteCategory")]
        public IActionResult DeleteCategoryPOST(int? categoryID)
        {
            CategoryModel? categoryObj = _unitOfWork.Category.Get(u => u.CategoryID == categoryID);
            if (categoryObj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(categoryObj);
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");
        }
        #endregion
    }
}

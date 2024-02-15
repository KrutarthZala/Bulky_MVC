using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Display Category
        public IActionResult Index()
        {
            List<CategoryModel> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }
        #endregion

        #region Create Category
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryModel categoryObj)
        {
            if (categoryObj.CategoryName == categoryObj.CategoryOrder.ToString())
            {
                ModelState.AddModelError("CategoryName", "The Category Display Order cannot exactly match the Category Name");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(categoryObj);
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
            if (categoryID == null || categoryID == 0)
            {
                return NotFound();
            }

            CategoryModel? categoryFromDb = _unitOfWork.Category.Get(u => u.CategoryID == categoryID);
            //CategoryModel? categoryFromDb1 = _dbCategory.Categories.FirstOrDefault(u=>u.CategoryID == categoryID);
            //CategoryModel? categoryFromDb2 = _dbCategory.Categories.Where(u => u.CategoryID == categoryID).FirstOrDefault();

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryModel categoryObj)
        {
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
            if (categoryID == null || categoryID == 0)
            {
                return NotFound();
            }

            CategoryModel? categoryFromDb = _unitOfWork.Category.Get(u => u.CategoryID == categoryID);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

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

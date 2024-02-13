using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbCategory;
        public CategoryController(ApplicationDbContext dbCategory) 
        {
            _dbCategory = dbCategory;
        }

        #region Display Category
        public IActionResult Index()
        {
            List<CategoryModel> objCategoryList = _dbCategory.Categories.ToList();
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
                _dbCategory.Categories.Add(categoryObj);
                _dbCategory.SaveChanges();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        #endregion

        #region Edit Category
        public IActionResult EditCategory(int? categoryID)
        {
            if(categoryID == null || categoryID == 0)
            {
                return NotFound();
            }

            CategoryModel? categoryFromDb = _dbCategory.Categories.Find(categoryID);
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
                _dbCategory.Categories.Update(categoryObj);
                _dbCategory.SaveChanges();
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

            CategoryModel? categoryFromDb = _dbCategory.Categories.Find(categoryID);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("DeleteCategory")]
        public IActionResult DeleteCategoryPOST(int? categoryID)
        {
            CategoryModel? categoryObj = _dbCategory.Categories.Find(categoryID);
            if (categoryObj == null)
            {
                return NotFound();
            }
            _dbCategory.Categories.Remove(categoryObj);
            _dbCategory.SaveChanges();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");
        }
        #endregion
    }
}

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
        public IActionResult Index()
        {
            List<CategoryModel> objCategoryList = _dbCategory.Categories.ToList();
            return View(objCategoryList);
        }
    }
}

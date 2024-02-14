using BulkyBookWebRazor_Temp.Data;
using BulkyBookWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyBookWebRazor_Temp.Pages.Category
{
    [BindProperties]
    public class DeleteCategoryModel : PageModel
    {
        private readonly ApplicationDbContext _dbCategory;
        public CategoryModel Category { get; set; }

        public DeleteCategoryModel(ApplicationDbContext dbCategory)
        {
            _dbCategory = dbCategory;
        }
        public void OnGet(int? categoryID)
        {
            if (categoryID != null && categoryID != 0)
            {
                Category = _dbCategory.Categories.Find(categoryID);
            }
        }

        public IActionResult OnPost()
        {
            CategoryModel? categoryObj = _dbCategory.Categories.Find(Category.CategoryID);
            if (categoryObj == null)
            {
                return NotFound();
            }
            _dbCategory.Categories.Remove(categoryObj);
            _dbCategory.SaveChanges();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToPage("Index");
        }
    }
}

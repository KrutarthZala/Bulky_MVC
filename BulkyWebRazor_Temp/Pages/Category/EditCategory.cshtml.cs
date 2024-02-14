using BulkyBookWebRazor_Temp.Data;
using BulkyBookWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyBookWebRazor_Temp.Pages.Category
{
    [BindProperties]
    public class EditCategoryModel : PageModel
    {
        private readonly ApplicationDbContext _dbCategory;
        public CategoryModel Category { get; set; }

        public EditCategoryModel(ApplicationDbContext dbCategory)
        {
            _dbCategory = dbCategory;
        }
        public void OnGet(int? categoryID)
        {
            if( categoryID != null && categoryID != 0 )
            {
                Category = _dbCategory.Categories.Find(categoryID);
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _dbCategory.Categories.Update(Category);
                _dbCategory.SaveChanges();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}

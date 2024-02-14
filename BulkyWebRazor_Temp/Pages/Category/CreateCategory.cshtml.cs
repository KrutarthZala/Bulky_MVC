using BulkyBookWebRazor_Temp.Data;
using BulkyBookWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyBookWebRazor_Temp.Pages.Category
{
    [BindProperties]
    public class CreateCategoryModel : PageModel
    {
        private readonly ApplicationDbContext _dbCategory;
        public CategoryModel Category { get; set; }

        public CreateCategoryModel(ApplicationDbContext dbCategory)
        {
            _dbCategory = dbCategory;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            _dbCategory.Add(Category);
            _dbCategory.SaveChanges();
            TempData["success"] = "Category Created Successfully";
            return RedirectToPage("Index");
        }
    }
}

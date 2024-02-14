using BulkyBookWebRazor_Temp.Data;
using BulkyBookWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace BulkyBookWebRazor_Temp.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbCategory;
        public List<CategoryModel> CategoryList { get; set; }

        public IndexModel(ApplicationDbContext dbCategory)
        {
            _dbCategory = dbCategory;
        }
        public void OnGet()
        {
               CategoryList = _dbCategory.Categories.ToList();
        }
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Models.ViewModels
{
    public class ProductVM
    {
        public ProductModel? Product { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? CategoryList {  get; set; } 
    }
}

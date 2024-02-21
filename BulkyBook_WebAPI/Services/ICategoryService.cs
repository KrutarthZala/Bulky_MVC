using BulkyBook_WebAPI.Models;

namespace BulkyBook_WebAPI.Services
{
    public interface ICategoryService  /*IService<CategoryModel>*/
    {
        void InsertCategory(CategoryModel category);
        void UpdateCategory(CategoryModel category);
    }
}

using BulkyBook_WebAPI.Models;

namespace BulkyBook_WebAPI.Services
{
    public interface ICategoryService : IService<CategoryModel>
    {
        void UpdateCategory(CategoryModel category);
    }
}

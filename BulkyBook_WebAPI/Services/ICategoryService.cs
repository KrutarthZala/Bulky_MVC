using BulkyBook_WebAPI.Models;

namespace BulkyBook_WebAPI.Services
{
    public interface ICategoryService /*: IService<CategoryModel>*/
    {
        Task <List<CategoryModel>> GetCategories();
        Task<CategoryModel?> GetCategory(int? categoryID);
        Task InsertCategory(CategoryModel category);
        Task UpdateCategory(CategoryModel category);
        Task DeleteCategory(CategoryModel category);
        Task SaveCategory();
    }
}

using BulkyBook_WebAPI.Models;
using System.Linq.Expressions;

namespace BulkyBook_WebAPI.Services
{
    public interface ICategoryService /*: IService<CategoryModel>*/
    {
        Task <List<CategoryModel>> GetCategories(); 
        CategoryModel GetCategory(CategoryModel category);
        Task InsertCategory(CategoryModel category);
        Task UpdateCategory(CategoryModel category);
        Task DeleteCategory(CategoryModel category);
        Task SaveCategory();
    }
}

using BulkyBook_WebAPI.Models;
using System.Linq.Expressions;

namespace BulkyBook_WebAPI.Services
{
    public interface ICategoryService /*: IService<CategoryModel>*/
    {
        IEnumerable<CategoryModel> GetCategories(); 
        CategoryModel GetCategory(Expression<Func<CategoryModel, bool>> filter);
        Task InsertCategory(CategoryModel category);
        Task UpdateCategory(CategoryModel category);
        Task DeleteCategory(CategoryModel category);
        Task SaveCategory();
    }
}

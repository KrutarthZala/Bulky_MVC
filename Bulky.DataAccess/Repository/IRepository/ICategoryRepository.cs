using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    // Implement IRepository Interface with Category Model
    public interface ICategoryRepository : IRepository<CategoryModel>
    {
        void Update(CategoryModel category);
    }
}

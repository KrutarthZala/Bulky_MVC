using BulkyBook_WebAPI.Data;
using BulkyBook_WebAPI.Models;
using BulkyBook_WebAPI.Services;

namespace BulkyBook_WebAPI.Implementation
{
    public class CategoryService : Service<CategoryModel>
    {
        private readonly ApplicationDbContext _db;
        public CategoryService(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void UpdateCategory(CategoryModel category)
        {
            _db.Category.Update(category);
        }
    }
}

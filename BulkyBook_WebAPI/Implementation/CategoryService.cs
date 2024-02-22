using BulkyBook_WebAPI.Data;
using BulkyBook_WebAPI.Models;
using BulkyBook_WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace BulkyBook_WebAPI.Implementation
{
    public class CategoryService : /*Service<CategoryModel> ,*/ ICategoryService
    {
        private readonly ApplicationDbContext _dbCategory;
        public CategoryService(ApplicationDbContext dbCategory) /*: base(db)*/
        {
            _dbCategory = dbCategory;
        }

        public IEnumerable<CategoryModel> GetCategories()
        {
            IQueryable<CategoryModel> query = _dbCategory.Set<CategoryModel>();
            return query.ToList();  
        }

        public CategoryModel GetCategory(Expression<Func<CategoryModel, bool>> filter)
        {
            IQueryable<CategoryModel> query = _dbCategory.Set<CategoryModel>();
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public async Task InsertCategory(CategoryModel category)
        {
            _dbCategory.Add(category);
        }

        public async Task UpdateCategory(CategoryModel category)
        {
            _dbCategory.Update(category);
            _dbCategory.SaveChanges();
        }
        public async Task DeleteCategory(CategoryModel category)
        {
            _dbCategory.Remove(category);
        }

        public async Task SaveCategory()
        {
            _dbCategory.SaveChanges();
        }
    }
}

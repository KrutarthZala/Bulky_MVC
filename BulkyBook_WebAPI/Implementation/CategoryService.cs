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

        public Task<List<CategoryModel>> GetCategories()
        {
            IQueryable<CategoryModel> query = _dbCategory.Set<CategoryModel>();
            return query.ToListAsync();  
        }

        public CategoryModel GetCategory(CategoryModel category)
        {
            //return _dbCategory.Where(c => c.CategoryID == category.CategoryID).FirstOrDefault();
            
        }

        public async Task InsertCategory(CategoryModel category)
        {
            await _dbCategory.AddAsync(category);
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

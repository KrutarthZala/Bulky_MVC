using BulkyBook_WebAPI.Data;
using BulkyBook_WebAPI.Models;
using BulkyBook_WebAPI.Services;
using Microsoft.EntityFrameworkCore;

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

        public async Task<CategoryModel?> GetCategory(int? categoryID)
        {
            return await  _dbCategory.Set<CategoryModel>().Where(c => c.CategoryID == categoryID).FirstOrDefaultAsync();
        }

        public async Task InsertCategory(CategoryModel category)
        {
            await _dbCategory.AddAsync(category);
        }

        public async Task UpdateCategory(CategoryModel category)
        {
            await Task.Run(() => _dbCategory.Update(category));
        }
        public async Task DeleteCategory(CategoryModel category)
        {
            await Task.Run(() => _dbCategory.Remove(category));
        }

        public async Task SaveCategory()
        {
            await _dbCategory.SaveChangesAsync();
        }
    }
}

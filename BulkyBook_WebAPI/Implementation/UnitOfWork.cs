using BulkyBook_WebAPI.Data;
using BulkyBook_WebAPI.Services;

namespace BulkyBook_WebAPI.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryService Category { get; private set; }
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryService(_db);
        }
        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}

using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class BulkyBookUserRepository : Repository<BulkyBookUser>, IBulkyBookUserRepository
    {
        private ApplicationDbContext _db;
        public BulkyBookUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

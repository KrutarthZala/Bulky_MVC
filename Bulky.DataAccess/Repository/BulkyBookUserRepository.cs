using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

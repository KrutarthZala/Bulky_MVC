using BulkyBook_WebAPI.Data;
using BulkyBook_WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BulkyBook_WebAPI.Implementation
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Service(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
            // _dbCategory.Categories == dbSet
        }
        public async Task Add(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        public async Task Remove(T entity)
        {
            await Task.Run(() => dbSet.Remove(entity));
        }
    }
}

using System.Linq.Expressions;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    // Use Generics because of multiple classes
    public interface IRepository<T> where T : class
    {
        // T - Category, T - Product, T - Company
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null, string? includeProperties = null);
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);

    }
}

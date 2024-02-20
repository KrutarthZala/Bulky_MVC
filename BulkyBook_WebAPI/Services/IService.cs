using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace BulkyBook_WebAPI.Services
{
    public interface IService<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>> filter);
        Task Add(T entity);
        Task Remove(T entity);
    }
}

using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    // Implement IRepository Interface with Product Model
    public interface IProductRepository : IRepository<ProductModel>
    {
        void Update(ProductModel product);
    }
}

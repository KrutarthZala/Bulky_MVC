using BulkyBook_WebAPI.Data;
using BulkyBook_WebAPI.Models;
using BulkyBook_WebAPI.Services;

namespace BulkyBook_WebAPI.Implementation
{
    public class ProductService : Service<ProductModel>, IProductService
    {
        private readonly ApplicationDbContext _db;
        public ProductService(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void UpdateProduct(ProductModel product)
        {
            throw new NotImplementedException();
        }
    }
}

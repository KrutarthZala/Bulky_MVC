using BulkyBook_WebAPI.Models;

namespace BulkyBook_WebAPI.Services
{
    public interface IProductService
    {
        //void UpdateProduct(ProductModel product);
        Task<List<ProductModel>> GetProducts();
        Task<ProductModel?> GetProduct(int? ProductID);
        Task InsertProduct(ProductModel Product);
        Task UpdateProduct(ProductModel Product);
        Task DeleteProduct(ProductModel Product);
        Task SaveProduct();
    }
}

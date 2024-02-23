using BulkyBook_WebAPI.Models;

namespace BulkyBook_WebAPI.Services
{
    public interface IProductService
    {
        //void UpdateProduct(ProductModel product);
        Task<List<ProductModel>> GetProducts();
        Task<ProductModel?> GetProduct(int? ProductID);
        Task InsertProduct(IFormFile image, string productDetailsJson);
        Task UpdateProduct(IFormFile image, string productDetailsJson, int productId);
        Task DeleteProduct(ProductModel Product);
        Task SaveProduct();
    }
}

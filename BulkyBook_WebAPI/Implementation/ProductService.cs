using BulkyBook_WebAPI.Data;
using BulkyBook_WebAPI.Models;
using BulkyBook_WebAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook_WebAPI.Implementation
{
    public class ProductService : /*Service<ProductModel>,*/ IProductService
    {
        private readonly ApplicationDbContext _dbProduct;
        public ProductService(ApplicationDbContext dbProduct) /*: base(db)*/
        {
            _dbProduct = dbProduct;
        }

        public async Task<List<ProductModel>> GetProducts()
        {
            return await _dbProduct.Set<ProductModel>().Include(p => p.Category).ToListAsync();
        }

        public async Task<ProductModel?> GetProduct(int? ProductID)
        {
            return await _dbProduct.Set<ProductModel>().Include(p => p.Category).Where(c => c.ProductID == ProductID).FirstOrDefaultAsync();
        }

        #region Insert Product
        public async Task InsertProduct(ProductModel product)
        {
            //await _dbProduct.AddAsync(Product);
            /*
            try
            {
                // Check if category exists
                var existingCategory = await _dbProduct.Set<CategoryModel>()
                    .FirstOrDefaultAsync(c => c.CategoryName == product.Category.CategoryName);

                if (existingCategory == null)
                {
                    throw new Exception($"Category with name '{product.Category.CategoryName}' does not exist.");
                }

                // Assign existing category to product
                product.Category = existingCategory;

                // Add product to the database
                await _dbProduct.AddAsync(product);
            }
            catch (Exception ex)
            {
                // Handle database exceptions or other errors
                throw new Exception("Failed to insert product", ex);
            }
            */
        }
        #endregion

        public async Task UpdateProduct(ProductModel product)
        { 
            await Task.Run(() => _dbProduct.Update(product));
        }
        public async Task DeleteProduct(ProductModel product)
        {
            await Task.Run(() => _dbProduct.Remove(product));
        }

        public async Task SaveProduct()
        {
            await _dbProduct.SaveChangesAsync();
        }


        //public void UpdateProduct(ProductModel product)
        //{
        //    throw new NotImplementedException();
        //}


    }
}

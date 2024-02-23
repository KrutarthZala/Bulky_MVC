using BulkyBook_WebAPI.Data;
using BulkyBook_WebAPI.Models;
using BulkyBook_WebAPI.Services;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BulkyBook_WebAPI.Implementation
{
    public class ProductService : /*Service<ProductModel>,*/ IProductService
    {
        private readonly ApplicationDbContext _dbProduct;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductService(ApplicationDbContext dbProduct, IWebHostEnvironment webHostEnvironment) /*: base(db)*/
        {
            _dbProduct = dbProduct;
            _webHostEnvironment = webHostEnvironment;   
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
        public async Task InsertProduct(IFormFile image, string productDetailsJson)
        {
            //await _dbProduct.AddAsync(Product);

            #region Normal Method
            // Normal Method
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
            #endregion

            #region Take image from user
            // Take Image From User
            try
            {
                // Parse JSON product details
                var product = JsonConvert.DeserializeObject<ProductModel>(productDetailsJson);

                // Check if category exists
                var existingCategory = await _dbProduct.Set<CategoryModel>()
                    .FirstOrDefaultAsync(c => c.CategoryName == product.Category.CategoryName);

                if (existingCategory == null)
                {
                    throw new Exception($"Category with name '{product.Category.CategoryName}' does not exist.");
                }

                // Assign existing category to product
                product.Category = existingCategory;

                // Process image
                if (image != null && image.Length > 0)
                {
                    // Generate a unique filename
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

                    // Set image path in product model
                    product.ProductImageURL = uniqueFileName;

                    // Save image to a designated folder (adjust path as needed)
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, @"images\product");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploadsFolder, uniqueFileName), FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                }

                await _dbProduct.AddAsync(product);
            }            
            catch (Exception ex)
            {
                throw new Exception("Failed to insert product", ex);
            }
            #endregion
        }
        #endregion

        #region Normal Update Method
        // Normal Method
        //public async Task UpdateProduct(ProductModel product)
        //{ 
        //    await Task.Run(() => _dbProduct.Update(product));
        //}
        #endregion

        #region Update product with image file
        public async Task UpdateProduct(IFormFile image, string productDetailsJson, int productId)
        {
            try
            {
                // Parse JSON product details
                var productToUpdate = JsonConvert.DeserializeObject<ProductModel>(productDetailsJson);

                // Check if category exists
                var existingCategory = await _dbProduct.Set<CategoryModel>()
                    .FirstOrDefaultAsync(c => c.CategoryName == productToUpdate.Category.CategoryName);

                if (existingCategory == null)
                {
                    throw new Exception($"Category with name '{productToUpdate.Category.CategoryName}' does not exist.");
                }

                // Fetch existing product
                var existingProduct = await _dbProduct.Set<ProductModel>()
                    .FindAsync(productId);

                if (existingProduct == null)
                {
                    throw new Exception($"Product with ID '{productId}' does not exist.");
                }

                // Update product properties from JSON
                existingProduct.ProductTitle = productToUpdate.ProductTitle;
                // Update other relevant properties based on your model

                // Update category
                existingProduct.Category = existingCategory;

                // Process image (assuming image property in ProductModel)
                if (image != null && image.Length > 0)
                {
                    // Save image to a designated folder (adjust path as needed)
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, @"images\product");
                    // Generate a unique filename
                    string uniqueFileName = null;
                    if (!string.IsNullOrEmpty(existingProduct.ProductImageURL))
                    {
                        // Delete old image if it exists
                        if (!string.IsNullOrEmpty(existingProduct.ProductImageURL))
                        {
                            var oldImagePath = Path.Combine(uploadsFolder, existingProduct.ProductImageURL);
                            if (File.Exists(oldImagePath))
                            {
                                File.Delete(oldImagePath);
                            }
                        }
                        uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    }

                    // Update image path
                    if (uniqueFileName != null)
                    {
                        existingProduct.ProductImageURL = uniqueFileName;
                    }

                    
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    if (uniqueFileName != null)
                    {
                        using (var fileStream = new FileStream(Path.Combine(uploadsFolder, uniqueFileName), FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }                        
                    }
                }

                // Update the product in the database
                _dbProduct.Update(existingProduct);
                await _dbProduct.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle database exceptions or other errors
                throw new Exception("Failed to update product", ex);
            }
        }
        #endregion

        #region Delete product with image
        public async Task DeleteProduct(ProductModel product)
        {
            // await Task.Run(() => _dbProduct.Remove(product));
            // Fetch existing product
            var existingProduct = await _dbProduct.Set<ProductModel>()
                .FindAsync(product.ProductID);

            if (!string.IsNullOrEmpty(existingProduct.ProductImageURL))
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, @"images\product");
                var oldImagePath = Path.Combine(uploadsFolder, existingProduct.ProductImageURL);
                if (File.Exists(oldImagePath))
                {
                    File.Delete(oldImagePath);
                }
            }
            await Task.Run(() => _dbProduct.Remove(product));

        }
        #endregion
        public async Task SaveProduct()
        {
            await _dbProduct.SaveChangesAsync();
        }

    }
}

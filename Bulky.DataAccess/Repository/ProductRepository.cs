using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<ProductModel>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        #region Update Product
        public void Update(ProductModel objProduct)
        {
            // Manually update product for Image URL and Safety
            var objFromDb = _db.Products.FirstOrDefault(u => u.ProductID == objProduct.ProductID);
            if (objFromDb != null)
            {
                objFromDb.ProductTitle = objProduct.ProductTitle;
                objFromDb.ProductDescription = objProduct.ProductDescription;
                objFromDb.ProductISBN = objProduct.ProductISBN;
                objFromDb.ProductAuthor = objProduct.ProductAuthor;
                objFromDb.ProductListPrice = objProduct.ProductListPrice;
                objFromDb.ProductPrice = objProduct.ProductPrice;
                objFromDb.ProductPrice50 = objProduct.ProductPrice50;
                objFromDb.ProductPrice100 = objProduct.ProductPrice100;
                objFromDb.CategoryID = objProduct.CategoryID;
                if(objFromDb.ProductImageURL != null)
                {
                    objFromDb.ProductImageURL = objProduct.ProductImageURL;
                }
            }
        }
        #endregion
    }
}

using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<ProductModel>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ProductModel objProduct)
        {
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
    }
}

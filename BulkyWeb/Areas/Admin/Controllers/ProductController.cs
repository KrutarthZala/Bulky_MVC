using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Display Product
        public IActionResult Index()
        {
            List<ProductModel> objProductList = _unitOfWork.Product.GetAll().ToList();
            return View(objProductList);
        }
        #endregion

        #region Create Product
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductModel ProductObj)
        {
            //if (ProductObj.ProductName == ProductObj.ProductOrder.ToString())
            //{
            //    ModelState.AddModelError("ProductName", "The Product Display Order cannot exactly match the Product Name");
            //}

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(ProductObj);
                _unitOfWork.Save();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        #endregion

        #region Edit Product
        public IActionResult EditProduct(int? ProductID)
        {
            if (ProductID == null || ProductID == 0)
            {
                return NotFound();
            }

            ProductModel? ProductFromDb = _unitOfWork.Product.Get(u => u.ProductID == ProductID);
            //ProductModel? ProductFromDb1 = _dbProduct.Product.FirstOrDefault(u=>u.ProductID == ProductID);
            //ProductModel? ProductFromDb2 = _dbProduct.Product.Where(u => u.ProductID == ProductID).FirstOrDefault();

            if (ProductFromDb == null)
            {
                return NotFound();
            }
            return View(ProductFromDb);
        }

        [HttpPost]
        public IActionResult EditProduct(ProductModel ProductObj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(ProductObj);
                _unitOfWork.Save();
                TempData["success"] = "Product Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        #endregion

        #region Delete Product
        public IActionResult DeleteProduct(int? ProductID)
        {
            if (ProductID == null || ProductID == 0)
            {
                return NotFound();
            }

            ProductModel? ProductFromDb = _unitOfWork.Product.Get(u => u.ProductID == ProductID);

            if (ProductFromDb == null)
            {
                return NotFound();
            }
            return View(ProductFromDb);
        }

        [HttpPost, ActionName("DeleteProduct")]
        public IActionResult DeleteProductPOST(int? ProductID)
        {
            ProductModel? ProductObj = _unitOfWork.Product.Get(u => u.ProductID == ProductID);
            if (ProductObj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(ProductObj);
            _unitOfWork.Save();
            TempData["success"] = "Product Deleted Successfully";
            return RedirectToAction("Index");
        }
        #endregion

    }
}

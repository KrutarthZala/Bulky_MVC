using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Display Product
        public IActionResult Index()
        {
            List<ProductModel> objProductList = _unitOfWork.Product.GetAll().ToList();
            
            return View(objProductList);
        }
        #endregion

        #region Create Product
        public IActionResult UpsertProduct(int? productID)
        {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.CategoryName,
                    Value = u.CategoryID.ToString()
                });

            // ViewBag.CategoryList = CategoryList;   
            // ViewData["CategoryList"] = CategoryList;

            ProductVM productVM = new()
            {
                CategoryList = CategoryList,
                Product = new ProductModel()
            };

            if( productID == null || productID == 0)
            {
                // Create 
                return View(productVM);
            }
            else
            {
                // Update
                productVM.Product = _unitOfWork.Product.Get(u=>u.ProductID ==  productID);
                return View(productVM);
            }
            
        }

        [HttpPost]
        public IActionResult UpsertProduct(ProductVM ProductObj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string productFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    using(var fileStream = new FileStream(Path.Combine(productPath, productFileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    ProductObj.Product.ProductImageURL = @"\images\product\" + productFileName;
                }
                _unitOfWork.Product.Add(ProductObj.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product Created Successfully";
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

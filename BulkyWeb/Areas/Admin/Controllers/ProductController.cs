using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    // Define Area and Authorize role.
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        // Instance of Dependency Injection
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
            // Retrieve List of products
            List<ProductModel> productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(productList);
        }
        #endregion

        #region Create and Edit / Upsert Product
        public IActionResult UpsertProduct(int? productID)
        {
            // Retrieve Category List for display Category Name
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.CategoryName,
                    Value = u.CategoryID.ToString()
                });

            // Example of ViewBag and ViewData
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
                // Derive path of wwwroot folder
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    // Assign new name to image
                    string productFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Save image path
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if(!string.IsNullOrEmpty(ProductObj.Product.ProductImageURL)) 
                    {
                        // Delete Old Image if user update image.
                        var oldImagePath = Path.Combine(wwwRootPath, ProductObj.Product.ProductImageURL.TrimStart('\\'));

                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using(var fileStream = new FileStream(Path.Combine(productPath, productFileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    ProductObj.Product.ProductImageURL = @"\images\product\" + productFileName;
                }

                if(ProductObj.Product.ProductID == 0)
                {
                    _unitOfWork.Product.Add(ProductObj.Product);
                    _unitOfWork.Save();
                    TempData["success"] = "Product Created Successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(ProductObj.Product);
                    _unitOfWork.Save();
                    TempData["success"] = "Product Updated Successfully";
                }
                return RedirectToAction("Index");
            }
            return View();
        }
        #endregion

        #region Product API Call
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            // Retrieve product list with category names
            List<ProductModel> productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            // Return list in JSON format
            return Json(new {data =  productList});

        }
        #endregion

        #region Delete Product
        [HttpDelete]
        public IActionResult DeleteProduct(int productID)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.ProductID == productID);
            if(productToBeDeleted == null)
            {
                return Json(new { success= false , message="Error While Deleting"});
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, (productToBeDeleted?.ProductImageURL ?? "").TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            if(productToBeDeleted != null)
            {
                _unitOfWork.Product.Remove(productToBeDeleted);
                _unitOfWork.Save(); 
            }

            return Json(new { success = true, message = "Product Deleted Successfully" });

        }
        #endregion
    }
}

﻿using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

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
            List<ProductModel> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            return View(objProductList);
        }
        #endregion

        #region Create and Edit / Upsert Product
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


        #region API Call
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            List<ProductModel> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data =  objProductList});

        }
        #endregion

        [HttpDelete]
        public IActionResult DeleteProduct(int productID)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.ProductID == productID);
            if(productToBeDeleted == null)
            {
                return Json(new { success= false , message="Error While Deleting"});
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ProductImageURL.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });

        }
    }
}

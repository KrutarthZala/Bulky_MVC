using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        // Display Home Page.
        public IActionResult Index()
        {
            IEnumerable<ProductModel> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(productList);
        }

        #region Details GET 
        // Display Details Page 
        public IActionResult Details(int productID)
        {
            ShoppingCartModel cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.ProductID == productID, includeProperties: "Category"),
                ProductCount = 1,
                ProductID = productID
            };
            return View(cart);
        }
        #endregion

        #region Details POST
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCartModel shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var bulkyBookUserID = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.BulkyBookUserID = bulkyBookUserID;

            ShoppingCartModel cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.BulkyBookUserID == bulkyBookUserID && u.ProductID == shoppingCart.ProductID);
            if (cartFromDb != null)
            {
                // Shopping cart exists
                cartFromDb.ProductCount += shoppingCart.ProductCount;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();
            }
            else
            {
                // Add Cart to Database
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.BulkyBookUserID == bulkyBookUserID).Count());

            }
            TempData["success"] = "Cart Updated Successfully";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
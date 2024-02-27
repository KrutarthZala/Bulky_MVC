using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var bulkyBookUserID = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.BulkyBookUserID == bulkyBookUserID, includeProperties: "Product"),
            };

            foreach(var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderTotal += (cart.Price * cart.ProductCount);
            }

            return View(ShoppingCartVM);
        }

        public IActionResult CartSummary()
        {
            return View();
        }

        #region Add Cart
        public IActionResult AddCart(int cartID)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.CartID == cartID);
            cartFromDb.ProductCount += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Subtract Cart
        public IActionResult SubtractCart(int cartID)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.CartID == cartID);
            if(cartFromDb.ProductCount <= 1)
            {
                // Remove from cart
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.ProductCount -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Remove Cart
        public IActionResult DeleteCart(int cartID)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.CartID == cartID);
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Get Price Based on Quantity
        private double GetPriceBasedOnQuantity(ShoppingCartModel shoppingCart)
        {
            if(shoppingCart.ProductCount <= 50)
            {
                return shoppingCart.Product.ProductPrice;
            }
            else
            {
                if(shoppingCart.ProductCount <= 100)
                {
                    return shoppingCart.Product.ProductPrice50;
                }
                else
                {
                    return shoppingCart.Product.ProductPrice100;
                }
            }
        }
        #endregion
    }
}

using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
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
        [BindProperty]
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
                OrderHeader = new()
            };

            foreach(var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.ProductCount);
            }

            return View(ShoppingCartVM);
        }

		#region Cart Summary GET
		public IActionResult CartSummary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var bulkyBookUserID = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.BulkyBookUserID == bulkyBookUserID, includeProperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.BulkyBookUser = _unitOfWork.BulkyBookUser.Get(u => u.Id == bulkyBookUserID);

            ShoppingCartVM.OrderHeader.UserName = ShoppingCartVM.OrderHeader.BulkyBookUser.BulkyBookUserName;
            ShoppingCartVM.OrderHeader.UserPhoneNumber = ShoppingCartVM.OrderHeader.BulkyBookUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.UserStreetAddress = ShoppingCartVM.OrderHeader.BulkyBookUser.UserStreetAddress;
            ShoppingCartVM.OrderHeader.UserCity = ShoppingCartVM.OrderHeader.BulkyBookUser.UserCity;
            ShoppingCartVM.OrderHeader.UserState = ShoppingCartVM.OrderHeader.BulkyBookUser.UserState;
            ShoppingCartVM.OrderHeader.UserPostalCode = ShoppingCartVM.OrderHeader.BulkyBookUser.UserPostalCode;


            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.ProductCount);
            }

            return View(ShoppingCartVM);
        }
		#endregion

		#region Cart Summary POST
		[HttpPost]
        [ActionName("CartSummary")]
		public IActionResult CartSummaryPOST()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var bulkyBookUserID = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.BulkyBookUserID == bulkyBookUserID, includeProperties: "Product");

            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.BulkyBookUserID = bulkyBookUserID;

			BulkyBookUser bulkyBookUser = _unitOfWork.BulkyBookUser.Get(u => u.Id == bulkyBookUserID);

			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.ProductCount);
			}

            if(bulkyBookUser.CompanyID.GetValueOrDefault() == 0)
            {
                // This is a regular customer account
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            }
            else
            {
                // This is company user
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
            }
            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            // Get Order Detail
            foreach(var cart in ShoppingCartVM.ShoppingCartList)
            {
                OrderDetailModel orderDetail = new()
                {
                    ProductID = cart.ProductID,
                    OrderHeaderID = ShoppingCartVM.OrderHeader.OrderHeaderID,
                    ProductPrice = cart.Price,
                    ProductCount = cart.ProductCount
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

			if (bulkyBookUser.CompanyID.GetValueOrDefault() == 0)
			{
				// This is a regular customer account
			}

			return RedirectToAction(nameof(OrderConformation), new { orderHeaderID = ShoppingCartVM.OrderHeader.OrderHeaderID});
		}
		#endregion

        public IActionResult OrderConformation(int orderHeaderID)
        {
            return View(orderHeaderID);
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

using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
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

		#region Index Method
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
		#endregion

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

			#region Stripe Logic For Payment
			if (bulkyBookUser.CompanyID.GetValueOrDefault() == 0)
			{
                // This is a regular customer account with stripe logic.
                var domain = "https://localhost:7163/";
				var options = new Stripe.Checkout.SessionCreateOptions
				{
					SuccessUrl = domain+$"Customer/Cart/OrderConformation?orderHeaderID={ShoppingCartVM.OrderHeader.OrderHeaderID}",
                    CancelUrl = domain+"Customer/Cart/Index",
					LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
					BillingAddressCollection = "required", // Collect the billing address
					ShippingAddressCollection = new SessionShippingAddressCollectionOptions(),
					Mode = "payment",
				};

                foreach(var item in ShoppingCartVM.ShoppingCartList)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100), // Rs. 20.50 => 2050
                            Currency = "inr",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.ProductTitle
                            }
                        },
                        Quantity = item.ProductCount
                    };
                    options.LineItems.Add(sessionLineItem);
                }

				var service = new Stripe.Checkout.SessionService();
				Session session = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.OrderHeaderID, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();
                Response.Headers.Add("Location",session.Url);
                return new StatusCodeResult(303);
			}
			#endregion

			return RedirectToAction(nameof(OrderConformation), new { orderHeaderID = ShoppingCartVM.OrderHeader.OrderHeaderID});
		}
		#endregion

		#region Order Conformation
		public IActionResult OrderConformation(int orderHeaderID)
        {
            OrderHeaderModel orderHeaderModel = _unitOfWork.OrderHeader.Get(u => u.OrderHeaderID == orderHeaderID,includeProperties:"BulkyBookUser");
            if(orderHeaderModel.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                // This is Ordered by Customer
                var service = new SessionService();
                Session session = service.Get(orderHeaderModel.PaymentSessionID);

                if(session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentID(orderHeaderID, session.Id,session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(orderHeaderID, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
                HttpContext.Session.Clear();
            }

            List<ShoppingCartModel> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.BulkyBookUserID == orderHeaderModel.BulkyBookUserID).ToList();

            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();

            return View(orderHeaderID);
        }
		#endregion

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
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.BulkyBookUserID == cartFromDb.BulkyBookUserID).Count() - 1);
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
            HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.BulkyBookUserID == cartFromDb.BulkyBookUserID).Count() - 1);
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

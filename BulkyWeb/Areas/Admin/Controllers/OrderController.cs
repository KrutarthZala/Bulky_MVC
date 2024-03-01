using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class OrderController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
		{
			return View();
		}

        #region Order Details
        public IActionResult OrderDetails(int orderID) 
        {
            OrderVM = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(u => u.OrderHeaderID == orderID, includeProperties: "BulkyBookUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderID == orderID, includeProperties: "Product")
            };
            return View(OrderVM);
        }
        #endregion

        #region Update Order Detail
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin+","+SD.Role_Employee)]
        public IActionResult UpdateOrderDetail()
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.OrderHeaderID == OrderVM.OrderHeader.OrderHeaderID);
            orderHeaderFromDb.UserName = OrderVM.OrderHeader.UserName;
            orderHeaderFromDb.UserPhoneNumber = OrderVM.OrderHeader.UserPhoneNumber;   
            orderHeaderFromDb.UserStreetAddress = OrderVM.OrderHeader.UserStreetAddress;
            orderHeaderFromDb.UserCity = OrderVM.OrderHeader.UserCity;  
            orderHeaderFromDb.UserState = OrderVM.OrderHeader.UserState;    
            orderHeaderFromDb.UserPostalCode = OrderVM.OrderHeader.UserPostalCode;
            if(!string.IsNullOrEmpty(OrderVM.OrderHeader.OrderCarrier))
            {
                orderHeaderFromDb.OrderCarrier = OrderVM.OrderHeader.OrderCarrier;
            }
            if(!string.IsNullOrEmpty(OrderVM.OrderHeader.OrderTrackingNumber))
            {
                orderHeaderFromDb.OrderTrackingNumber = OrderVM.OrderHeader.OrderTrackingNumber;
            }
            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();
            TempData["success"] = "Order Details Updated Successfully.";

            return RedirectToAction(nameof(OrderDetails),new { orderID = orderHeaderFromDb.OrderHeaderID});
        }
        #endregion

        #region Start Processing
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.OrderHeaderID,SD.StatusInProcess);
            _unitOfWork.Save();
            TempData["success"] = "Order is in Processing.";
            return RedirectToAction(nameof(OrderDetails), new { orderID = OrderVM.OrderHeader.OrderHeaderID });
        }
        #endregion

        #region Ship Order
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult ShipOrder()
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.OrderHeaderID == OrderVM.OrderHeader.OrderHeaderID);
            orderHeader.OrderTrackingNumber = OrderVM.OrderHeader.OrderTrackingNumber;
            orderHeader.OrderCarrier = OrderVM.OrderHeader.OrderCarrier;
            orderHeader.OrderStatus = SD.StatusShipped;
            orderHeader.ShippingDate = DateTime.Now;
            if(orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                orderHeader.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
            }


            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();
            TempData["success"] = "Order Shipped Successfully";
            return RedirectToAction(nameof(OrderDetails), new { orderID = OrderVM.OrderHeader.OrderHeaderID });
        }
        #endregion

        #region Cancel Order
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult CancelOrder()
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.OrderHeaderID == OrderVM.OrderHeader.OrderHeaderID);

            if(orderHeader.PaymentStatus == SD.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentID,
                };

                var service = new RefundService();
                Refund refund = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.OrderHeaderID, SD.StatusCancelled, SD.StatusCancelled);
            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.OrderHeaderID, SD.StatusCancelled, SD.StatusCancelled);
            }
            _unitOfWork.Save();
            TempData["success"] = "Order Cancelled Successfully";
            return RedirectToAction(nameof(OrderDetails), new { orderID = OrderVM.OrderHeader.OrderHeaderID });
        }
        #endregion

        #region Company Payment
        [HttpPost]
        [ActionName("OrderDetails")]
        public IActionResult Details_Pay_Now()
        {
            OrderVM.OrderHeader = _unitOfWork.OrderHeader.Get(u => u.OrderHeaderID == OrderVM.OrderHeader.OrderHeaderID, includeProperties: "BulkyBookUser");
            OrderVM.OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderID == OrderVM.OrderHeader.OrderHeaderID, includeProperties: "Product");

            // This is a company account with stripe logic.
            var domain = "https://localhost:7163/";
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = domain + $"Admin/Order/PaymentConformation?orderHeaderID={OrderVM.OrderHeader.OrderHeaderID}",
                CancelUrl = domain + $"Admin/Order/OrderDetails?orderID={OrderVM.OrderHeader.OrderHeaderID}",
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                BillingAddressCollection = "required", // Collect the billing address
                ShippingAddressCollection = new SessionShippingAddressCollectionOptions(),
                Mode = "payment",
            };

            foreach (var item in OrderVM.OrderDetail)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.ProductPrice * 100), // Rs. 20.50 => 2050
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
            _unitOfWork.OrderHeader.UpdateStripePaymentID(OrderVM.OrderHeader.OrderHeaderID, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        #endregion

        #region Payment Conformation
        public IActionResult PaymentConformation(int orderHeaderID)
        {
            OrderHeaderModel orderHeaderModel = _unitOfWork.OrderHeader.Get(u => u.OrderHeaderID == orderHeaderID);
            if (orderHeaderModel.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                // This is Ordered by Company
                var service = new SessionService();
                Session session = service.Get(orderHeaderModel.PaymentSessionID);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentID(orderHeaderID, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(orderHeaderID, orderHeaderModel.OrderStatus, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }

            return View(orderHeaderID);
        }
        #endregion

        #region Order API Call
        [HttpGet]
		public IActionResult GetAllOrders(string orderStatus)
		{
			// Retrieve product list with category names
			IEnumerable<OrderHeaderModel> orderList;

            if(User.IsInRole(SD.Role_Admin)||User.IsInRole(SD.Role_Employee))
            {
                orderList = _unitOfWork.OrderHeader.GetAll(includeProperties: "BulkyBookUser").ToList();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var bulkyBookUserID = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                orderList = _unitOfWork.OrderHeader.GetAll(u => u.BulkyBookUserID == bulkyBookUserID, includeProperties: "BulkyBookUser");
            }


            switch (orderStatus)
            {
                case "inprocess":
                    orderList =orderList.Where(o => o.OrderStatus == SD.StatusInProcess);
                    break;
                case "pending":
                    orderList = orderList.Where(o => o.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "completed":
                    orderList = orderList.Where(o => o.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    orderList = orderList.Where(o => o.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;
            }

            // Return list in JSON format
            return Json(new { data = orderList });

		}
		#endregion
	}
}

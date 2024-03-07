using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReportController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult OrderReport()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetOrderReport(DateTime startOrderDate, DateTime endOrderDate)
        {
            var orderList = _unitOfWork.OrderHeader.GetAll(includeProperties: "BulkyBookUser");

            orderList = orderList.Where(o => o.OrderDate.Date >= startOrderDate.Date 
                                     && o.OrderDate.Date <= endOrderDate.Date).ToList();
            return Json(new { data = orderList });
        }


        public IActionResult TransactionReport()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetTransactionReport(DateTime startOrderDate, DateTime endOrderDate)
        {
            var orderList = _unitOfWork.OrderHeader.GetAll(includeProperties: "BulkyBookUser");

            orderList = orderList.Where(o => o.PaymentDate.Date >= startOrderDate.Date 
                            && o.PaymentDate.Date <= endOrderDate.Date 
                            && (o.OrderStatus == SD.StatusApproved || 
                                o.OrderStatus == SD.StatusInProcess || 
                                o.OrderStatus == SD.StatusShipped)
                            && o.PaymentStatus == SD.PaymentStatusApproved).ToList();
            return Json(new { data = orderList });
        }
    }
}

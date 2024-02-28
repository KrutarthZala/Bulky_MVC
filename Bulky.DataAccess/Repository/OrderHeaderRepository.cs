using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeaderModel>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeaderModel orderHeader)
        {
            _db.OrderHeaders.Update(orderHeader);
        }

		public void UpdateStatus(int orderHeaderID, string orderStatus, string? paymentStatus = null)
		{
			var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.OrderHeaderID == orderHeaderID);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if(!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
		}

		public void UpdateStripePaymentID(int orderHeaderID, string paymentSessionID, string paymentIntentID)
		{
			var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.OrderHeaderID == orderHeaderID);
            if(!string.IsNullOrEmpty(paymentSessionID))
            {
                orderFromDb.PaymentSessionID = paymentSessionID;    
            }
            if(!string.IsNullOrEmpty(paymentIntentID))
            {
                orderFromDb.PaymentIntentID = paymentIntentID;
                orderFromDb.PaymentDate = DateTime.Now;
            }
		}
	}
}

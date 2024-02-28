using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeaderModel>
    {
        void Update(OrderHeaderModel orderHeader);
        void UpdateStatus(int orderHeaderID, string orderStatus, string? paymentStatus = null);
        void UpdateStripePaymentID(int  orderHeaderID, string paymentSessionID, string paymentIntentID);
    }
}

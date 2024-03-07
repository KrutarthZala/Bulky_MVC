using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeaderModel>
    {
        void Update(OrderHeaderModel orderHeader);
        void UpdateStatus(int orderHeaderID, string orderStatus, string? paymentStatus = null);
        void UpdateStripePaymentID(int  orderHeaderID, string paymentSessionID, string paymentIntentID);
    }
}

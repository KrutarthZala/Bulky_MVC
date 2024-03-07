using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepository : IRepository<OrderDetailModel>
    {
        void Update(OrderDetailModel orderDetail);
    }
}

using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCartModel>
    {
        void Update(ShoppingCartModel shoppingCart);
    }
}

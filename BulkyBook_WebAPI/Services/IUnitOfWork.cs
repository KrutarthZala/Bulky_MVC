namespace BulkyBook_WebAPI.Services
{
    public interface IUnitOfWork
    {
        ICategoryService Category {  get; }
        IProductService Product { get; }
        Task Save();
    }
}

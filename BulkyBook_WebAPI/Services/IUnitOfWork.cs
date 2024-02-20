namespace BulkyBook_WebAPI.Services
{
    public interface IUnitOfWork
    {
        ICategoryService Category {  get; }
        Task Save();
    }
}

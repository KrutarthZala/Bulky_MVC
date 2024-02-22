using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    // Implement IRepository Interface with Company Model
    public interface ICompanyRepository : IRepository<CompanyModel>
    {
        void Update(CompanyModel company);
    }
}

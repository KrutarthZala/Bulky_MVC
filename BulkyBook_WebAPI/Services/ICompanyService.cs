using BulkyBook_WebAPI.Models;

namespace BulkyBook_WebAPI.Services
{
    public interface ICompanyService
    {
        Task<List<CompanyModel>> GetCategories();
        Task<CompanyModel?> GetCompany(int? CompanyID);
        Task InsertCompany(CompanyModel Company);
        Task UpdateCompany(CompanyModel Company);
        Task DeleteCompany(CompanyModel Company);
        Task SaveCompany();
    }
}

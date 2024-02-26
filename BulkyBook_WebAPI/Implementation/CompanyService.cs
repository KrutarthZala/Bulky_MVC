using BulkyBook_WebAPI.Data;
using BulkyBook_WebAPI.Models;
using BulkyBook_WebAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook_WebAPI.Implementation
{
    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDbContext _dbCompany;
        public CompanyService(ApplicationDbContext dbCompany) /*: base(db)*/
        {
            _dbCompany = dbCompany;
        }

        public Task<List<CompanyModel>> GetCategories()
        {
            IQueryable<CompanyModel> query = _dbCompany.Set<CompanyModel>();
            return query.ToListAsync();
        }

        public async Task<CompanyModel?> GetCompany(int? CompanyID)
        {
            return await _dbCompany.Set<CompanyModel>().Where(c => c.CompanyID == CompanyID).FirstOrDefaultAsync();
        }

        public async Task InsertCompany(CompanyModel Company)
        {
            await _dbCompany.AddAsync(Company);
        }

        public async Task UpdateCompany(CompanyModel Company)
        {
            await Task.Run(() => _dbCompany.Update(Company));
        }
        public async Task DeleteCompany(CompanyModel Company)
        {
            await Task.Run(() => _dbCompany.Remove(Company));
        }

        public async Task SaveCompany()
        {
            await _dbCompany.SaveChangesAsync();
        }
    }
}

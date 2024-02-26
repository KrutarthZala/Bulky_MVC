using BulkyBook_WebAPI.Models;
using BulkyBook_WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        #region Company GET
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                //var categories = _unitOfWork.Company.GetAll();
                var categories = await _companyService.GetCategories();

                // Validate categories
                if (categories == null || !categories.Any())
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "No categories found"
                    });
                }

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Success",
                    Data = categories
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }
        #endregion

        #region Company GET with Specific CompanyID
        [HttpGet("{CompanyID}")]
        public async Task<IActionResult> GetSpecificCompany(int CompanyID)
        {
            // Validate CompanyID
            if (CompanyID < 1)
            {
                return BadRequest(new { StatusCode = 400, Message = "Bad Request" });
            }

            //var Company = _unitOfWork.Company.Get(p => p.CompanyID == CompanyID);
            var Company = await _companyService.GetCompany(CompanyID);

            // Validate Company
            if (Company == null)
            {
                return NotFound(new { StatusCode = 404, Message = "Not Found" });
            }
            return Ok(new { StatusCode = 200, Message = "Success", Data = Company });
        }
        #endregion

        #region Company POST / Add Company
        [HttpPost]
        public async Task<IActionResult> PostCompany(CompanyModel Company)
        {
            try
            {
                // Validate Company
                if (Company == null)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Bad Request"
                    });
                }

                //await _unitOfWork.Company.Add(Company);
                //await _unitOfWork.Save();

                await _companyService.InsertCompany(Company);
                await _companyService.SaveCompany();
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Company inserted successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(406, new { Message = "Not Acceptable", Details = ex.Message });
            }
        }
        #endregion

        #region Company PUT / Update Company
        [HttpPut]
        public async Task<IActionResult> PutCompany(CompanyModel Company)
        {
            // Validate CompanyID and Company
            if (Company == null || Company.CompanyID == 0)
            {
                return BadRequest(new { StatusCode = 400, Message = "Bad Request" });
            }

            //newCompany.CompanyName = Company.CompanyName;
            //newCompany.CompanyOrder = Company.CompanyOrder;

            // await _unitOfWork.Save();
            await _companyService.UpdateCompany(Company);
            await _companyService.SaveCompany();
            return Ok(new { StatusCode = 200, Message = "Company Updated successfully" });
        }
        #endregion

        #region Company DELETE
        [HttpDelete("{CompanyID}")]
        public async Task<IActionResult> DeleteProduct(int CompanyID)
        {
            // Validate CompanyID
            if (CompanyID < 1)
            {
                return BadRequest(new { StatusCode = 400, Message = "Bad Request" });
            }

            var Company = await _companyService.GetCompany(CompanyID);

            // Validate Company
            if (Company == null)
            {
                return NotFound(new { StatusCode = 404, Message = "Not Found" });
            }
            //await _unitOfWork.Company.Remove(Company);
            //await _unitOfWork.Save();
            await _companyService.DeleteCompany(Company);
            await _companyService.SaveCompany();
            return Ok(new { StatusCode = 200, Message = "Company deleted successfully" });
        }
        #endregion
    }
}

using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    // Define Area and Authorize role.
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        // Instance of Dependency Injection
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Display Company
        public IActionResult Index()
        {
            // Retrieve list of Company
            List<CompanyModel> companyList = _unitOfWork.Company.GetAll().ToList();
            return View(companyList);
        }
        #endregion

        #region Create and Edit / Upsert Company
        public IActionResult UpsertCompany(int? CompanyID)
        {
            // Check the value of CompanyID
            if( CompanyID == null || CompanyID < 0)
            {
                // Return Create View
                return View(new CompanyModel());
            }
            else
            {
                // Return Update View
                CompanyModel company = _unitOfWork.Company.Get(u=>u.CompanyID ==  CompanyID);
                return View(company);
            }            
        }

        [HttpPost]
        public IActionResult UpsertCompany(CompanyModel CompanyObj)
        {
            if (ModelState.IsValid)
            {                
                if(CompanyObj.CompanyID == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                    _unitOfWork.Save();
                    TempData["success"] = "Company Created Successfully";
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                    _unitOfWork.Save();
                    TempData["success"] = "Company Updated Successfully";
                }
                return RedirectToAction("Index");
            }
            return View();
        }
        #endregion

        #region Company API Call
        [HttpGet]
        public IActionResult GetAllCompanies()
        {
            // Retrieve Company List and return as JSON format.
            List<CompanyModel> companyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new {data = companyList });

        }
        #endregion

        #region Company Delete
        [HttpDelete]
        public IActionResult DeleteCompany(int CompanyID)
        {
            var companyToBeDeleted = _unitOfWork.Company.Get(u => u.CompanyID == CompanyID);
            if(companyToBeDeleted == null)
            {
                return Json(new { success= false , message="Error While Deleting"});
            }
            _unitOfWork.Company.Remove(companyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Company Deleted Successfully" });
        }
        #endregion
    }
}

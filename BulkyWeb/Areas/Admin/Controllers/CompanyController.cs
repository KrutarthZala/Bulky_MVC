using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Display Company
        public IActionResult Index()
        {
            List<CompanyModel> objCompanyList = _unitOfWork.Company.GetAll().ToList();

            return View(objCompanyList);
        }
        #endregion

        #region Create and Edit / Upsert Company
        public IActionResult UpsertCompany(int? CompanyID)
        {

            if( CompanyID == null || CompanyID == 0)
            {
                // Create 
                return View(new CompanyModel());
            }
            else
            {
                // Update
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
            List<CompanyModel> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new {data =  objCompanyList});

        }
        #endregion

        #region Company Delete
        [HttpDelete]
        public IActionResult DeleteCompany(int CompanyID)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.CompanyID == CompanyID);
            if(CompanyToBeDeleted == null)
            {
                return Json(new { success= false , message="Error While Deleting"});
            }

            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Company Deleted Successfully" });

        }
        #endregion
    }
}

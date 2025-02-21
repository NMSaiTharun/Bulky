using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult Index()
        {
            List<Company> CompanyList = _unitOfWork.Company.GetAll().ToList();
            return View(CompanyList);
        }
        public IActionResult Upsert(int? id)
        {            
            
            if (id == null || id == 0)
            {
                return View(new Company());
            }
            else
            {
              Company companyObj = _unitOfWork.Company.Get(a => a.Id == id);
                return View(companyObj);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {

            if (ModelState.IsValid)
            {                
                if (companyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(companyObj);
                    TempData["Success"] = "Company created successfully!";
                }
                else
                {
                    _unitOfWork.Company.Update(companyObj);
                    TempData["Success"] = "Company updated successfully!";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {            

                return View(companyObj);
            }

        }    
      
        #region API_CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> CompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = CompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(a => a.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            else
            {                
                _unitOfWork.Company.Remove(CompanyToBeDeleted);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Delete Successful" });
            }

        }
        #endregion
    }
}

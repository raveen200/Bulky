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
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWorks _unitOfWorks;

        public CompanyController(IUnitOfWorks unitOfWorks )
        {
            _unitOfWorks = unitOfWorks;
           

        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWorks.Company.GetAll().ToList();

            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {
            
            if (id == null || id == 0)
            {
                return View(new Company());
            }
            else
            {
               Company companyObj = _unitOfWorks.Company.Get(u => u.Id == id);
                return View(companyObj);
            }


        }

        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {


            if (ModelState.IsValid)
            {

                if (companyObj.Id != 0)
                {
                    _unitOfWorks.Company.Update( companyObj);
                }
                else
                {
                    _unitOfWorks.Company.Add(companyObj);
                }
                _unitOfWorks.Save();
                TempData["success"] = "Company Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
              
                return View(companyObj);

            }

        }





        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWorks.Company.GetAll().ToList();

            return Json(new { data = objCompanyList });
        }



        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWorks.Company.Get(u => u.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWorks.Company.Remove(CompanyToBeDeleted);
            _unitOfWorks.Save();

            return Json(new { success = true, message = "Delete Successful" });


          
        }
        #endregion
    }
}

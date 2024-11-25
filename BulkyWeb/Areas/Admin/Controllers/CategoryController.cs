using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWorks _unitOfWorks;
        public CategoryController(IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;

        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWorks.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Name and Display Order can not be same");
            }


            if (ModelState.IsValid)
            {

                _unitOfWorks.Category.Add(obj);
                _unitOfWorks.Save();
                TempData["success"] = "Category Created Succesfully";
                return RedirectToAction("Index");
            }

            return View();

        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //Category? categoryFormDb = _db.Categories.Find(id);
            Category? categoryFormDb = _unitOfWorks.Category.Get(u => u.Id == id);
            if (categoryFormDb == null)
            {
                return NotFound();
            }
            return View(categoryFormDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {

                _unitOfWorks.Category.Update(obj);
                _unitOfWorks.Save();
                TempData["success"] = "Category Updated Succesfully";
                return RedirectToAction("Index");
            }

            return View();

        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //Category? categoryFormDb = _db.Categories.Find(id);
            Category? categoryFormDb = _unitOfWorks.Category.Get(u => u.Id == id);
            if (categoryFormDb == null)
            {
                return NotFound();
            }
            return View(categoryFormDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitOfWorks.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWorks.Category.Remove(obj);
            _unitOfWorks.Save();
            TempData["success"] = "Category Delete Succesfully";
            return RedirectToAction("Index");




        }
    }
}

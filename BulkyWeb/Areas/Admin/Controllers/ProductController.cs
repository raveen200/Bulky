using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWorks _unitOfWorks;
        public ProductController(IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;

        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWorks.Product.GetAll().ToList();
           
            return View(objProductList);
        }

        public IActionResult Create()
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWorks.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Product = new Product()
            };
            return View(productVM);
        }

        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {


            if (ModelState.IsValid)
            {

                _unitOfWorks.Product.Add(productVM.Product);
                _unitOfWorks.Save();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWorks.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                return View(productVM);

            }
              
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? productFormDb = _unitOfWorks.Product.Get(u => u.Id == id);
            if (productFormDb == null)
            {
                return NotFound();
            }
            return View(productFormDb);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {

                _unitOfWorks.Product.Update(obj);
                _unitOfWorks.Save();
                TempData["success"] = "Product Updated Succesfully";
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

            Product? productFormDb = _unitOfWorks.Product.Get(u => u.Id == id);
            if (productFormDb == null)
            {
                return NotFound();
            }
            return View(productFormDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWorks.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWorks.Product.Remove(obj);
            _unitOfWorks.Save();
            TempData["success"] = "Product Delete Succesfully";
            return RedirectToAction("Index");




        }
    }
}

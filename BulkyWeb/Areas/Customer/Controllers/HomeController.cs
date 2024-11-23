
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWorks _unitOfWorks;

        public HomeController(ILogger<HomeController> logger,IUnitOfWorks unitOfWorks)
        {
            _logger = logger;
            _unitOfWorks = unitOfWorks;

        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWorks.Product.GetAll(includeProperties: "Category");
            return View(productList);
        }

        public IActionResult Details( int productId)
        {
            Product product = _unitOfWorks.Product.Get(u=>u.Id == productId, includeProperties: "Category");
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

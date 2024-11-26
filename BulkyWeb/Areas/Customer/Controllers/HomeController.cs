
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

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
            ShoppingCart cart = new()
            {
                Product  = _unitOfWorks.Product.Get(u=>u.Id == productId, includeProperties: "Category"),
                Count =1,
                ProductId=productId




            };
           
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var useId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = useId;

            ShoppingCart cartFromDb = _unitOfWorks.ShoppingCart.Get(u => u.ApplicationUserId == useId && u.ProductId == shoppingCart.ProductId);

            if(cartFromDb != null)
            {
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWorks.ShoppingCart.Update(cartFromDb);

            }
            else
            {
                _unitOfWorks.ShoppingCart.Add(shoppingCart);
            }

           
           
            _unitOfWorks.Save();
            
            return RedirectToAction(nameof(Index));
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

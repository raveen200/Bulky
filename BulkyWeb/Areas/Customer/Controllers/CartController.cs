using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWorks _unitOfWorks;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;
        }
        public IActionResult Index()
        {


            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var useId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCartList = _unitOfWorks.ShoppingCart.GetAll(u => u.ApplicationUserId == useId, includeProperties: "Product")

            };

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderTotal += (cart.Price * cart.Count);
            }


            return View(ShoppingCartVM);
        }

        public IActionResult Summary()
        {
            return View();
        }


        public IActionResult Plus(int CartId)
        {
            var CartFormDb = _unitOfWorks.ShoppingCart.Get(u => u.Id == CartId);
            CartFormDb.Count += 1;
            _unitOfWorks.ShoppingCart.Update(CartFormDb);
            _unitOfWorks.Save();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Minus(int CartId)
        {
            var CartFormDb = _unitOfWorks.ShoppingCart.Get(u => u.Id == CartId);
            if (CartFormDb.Count <= 1)
            {
                _unitOfWorks.ShoppingCart.Remove(CartFormDb);
            }
            else
            {
                CartFormDb.Count -= 1;
                _unitOfWorks.ShoppingCart.Update(CartFormDb);
            }

            _unitOfWorks.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int CartId)
        {
            var CartFormDb = _unitOfWorks.ShoppingCart.Get(u => u.Id == CartId);

            _unitOfWorks.ShoppingCart.Remove(CartFormDb);
            _unitOfWorks.Save();
            return RedirectToAction(nameof(Index));

        }



        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count == 50)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if (shoppingCart.Count == 100)
                {
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;
                }

            }
        }
    }
}

using System.Diagnostics;
using System.Security.Claims;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {           
            IEnumerable<Product> productList=_unitOfWork.Product.GetAll(includeProperties:"Category");
            //<img src="~/images/book.png" style="width:30px"/>
            return View(productList);
        }
        public IActionResult Details(int id)
        {
            ShoppingCart shoppingCart = new() 
            {
                Product = _unitOfWork.Product.Get(a => a.Id == id, includeProperties: "Category"),
                Count=1,
                ProductId=id
            };           
            return View(shoppingCart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCartObj)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCartObj.ApplicationUserId = userId;
            var cartFromDb = _unitOfWork.ShoppingCart.Get(a => a.ApplicationUserId == userId && a.ProductId == shoppingCartObj.ProductId);
            if (cartFromDb != null)
            {
                cartFromDb.Count += shoppingCartObj.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();
                TempData["success"] = "Updated to Cart Successfully!";
            }
            else
            {
                shoppingCartObj.Id = 0;
                _unitOfWork.ShoppingCart.Add(shoppingCartObj);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(a => a.ApplicationUserId == userId).Count());
                TempData["success"] = "Added to Cart Successfully!";
            }                          
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

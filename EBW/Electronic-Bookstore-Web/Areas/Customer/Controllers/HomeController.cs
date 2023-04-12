
using EBW.DataAccess;
using EBW.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Electronic_Bookstore_Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
		}

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> curentProductList = await _unitOfWork.Product.GetAllAsync("Author", "Category", "CoverType");

            return View(curentProductList);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            ShoppingCart shoppingCartObj = new()
            {
                Id = default,
                Product = await _unitOfWork.Product.GetFirstOrDefaultAsync(u => u.Id == id, "Author", "Category", "CoverType"),
                Count = 1,
                ProductId = id
            };
            return View(shoppingCartObj);
	    }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Details(ShoppingCart shoppingCartObj)
        {
            shoppingCartObj.Id = default;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            shoppingCartObj.ApplicationUserId = userId;

            ShoppingCart cartObjFromDB = await _unitOfWork.ShoppingCart.GetFirstOrDefaultAsync( x => x.ApplicationUserId == userId && x.ProductId == shoppingCartObj.ProductId);
            if(cartObjFromDB != null) 
            {
                cartObjFromDB.Count += shoppingCartObj.Count;
                await _unitOfWork.ShoppingCart.UpdateAsync(cartObjFromDB);
				TempData["success"] = "Product update to card";
			}
            else
            {
				await _unitOfWork.ShoppingCart.AddAsync(shoppingCartObj);
				TempData["success"] = "Product added to card";
			}
			await _unitOfWork.SaveAsync();
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

using EBW.DataAccess;
using EBW.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> curentProductList = await _unitOfWork.Product.GetAllAsync("Author", "Category", "CoverType");

            return View(curentProductList);
        }
        public async Task<IActionResult> Details(int id)
        {
            ShoppingCart cardObj = new()
            {
                Product = await _unitOfWork.Product.GetFirstOrDefaultAsync(u => u.Id == id, "Author", "Category", "CoverType")
            };

            return View(cardObj);
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
using EBW.DataAccess;
using EBW.Models;
using EBW.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Security.Claims;

namespace Electronic_Bookstore_Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ShoppingCartVM = new()
            {
                ShoppingCartList = await _unitOfWork.ShoppingCart.GetAllAsync(x => x.ApplicationUserId == userId,"Product"),
                OrderUserInfo = new()
            };
            ShoppingCartVM.OrderUserInfo.TotalOrderPrice =  ShoppingCartVM.ShoppingCartList.Select(x => (
            x.Product.Price == null ? x.Product.ListPrice : (decimal)x.Product.Price) * x.Count)
            .Sum();

            return View(ShoppingCartVM);
        }

        [ActionName("Plus")]
        public async Task<IActionResult> Plus(int cardObjId)
        {
            var cardFromDB = await _unitOfWork.ShoppingCart.GetFirstOrDefaultAsync(x => x.Id == cardObjId);
            cardFromDB.Count+=1;
            await _unitOfWork.ShoppingCart.UpdateAsync(cardFromDB);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
        [ActionName("Minus")]
        public async Task<IActionResult> Minus(int cardObjId)
        {
            var cardFromDB = await _unitOfWork.ShoppingCart.GetFirstOrDefaultAsync(x => x.Id == cardObjId);
            if(cardFromDB.Count > 1)
            {
                cardFromDB.Count-=1;
            }
             await _unitOfWork.ShoppingCart.UpdateAsync(cardFromDB);
             await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int cardObjId)
        {
            var cardFromDB = await _unitOfWork.ShoppingCart.GetFirstOrDefaultAsync(x => x.Id == cardObjId);
            await _unitOfWork.ShoppingCart.RemoveAsync(cardObjId);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = await _unitOfWork.ShoppingCart.GetAllAsync(x => x.ApplicationUserId == userId, "Product"),
                OrderUserInfo = new(),
            };
            
            ShoppingCartVM.OrderUserInfo.ApplicationUser = await _userManager.GetUserAsync(User);
            ShoppingCartVM.OrderUserInfo.FirstName = ShoppingCartVM.OrderUserInfo.ApplicationUser.FirstName;
            ShoppingCartVM.OrderUserInfo.LastName = ShoppingCartVM.OrderUserInfo.ApplicationUser.LastName;
            ShoppingCartVM.OrderUserInfo.City = ShoppingCartVM.OrderUserInfo.ApplicationUser.City;
            ShoppingCartVM.OrderUserInfo.BranchOffice = ShoppingCartVM.OrderUserInfo.ApplicationUser.BranchOffice;
            ShoppingCartVM.OrderUserInfo.PhoneNumber = ShoppingCartVM.OrderUserInfo.ApplicationUser.PhoneNumber;

            ShoppingCartVM.OrderUserInfo.TotalOrderPrice = ShoppingCartVM.ShoppingCartList.Select(x => (
            x.Product.Price == null ? x.Product.ListPrice : (decimal)x.Product.Price) * x.Count)
            .Sum();

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPostAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ShoppingCartVM.ShoppingCartList = await _unitOfWork.ShoppingCart.GetAllAsync(x => x.ApplicationUserId == userId , "Product");
            ShoppingCartVM.OrderUserInfo.OrderDate= DateTime.Now;
            ShoppingCartVM.OrderUserInfo.ApplicationUserId = userId;
            ShoppingCartVM.OrderUserInfo.ApplicationUser = await _userManager.GetUserAsync(User);

            ShoppingCartVM.OrderUserInfo.TotalOrderPrice = ShoppingCartVM.ShoppingCartList.Select(x => (
            x.Product.Price == null ? x.Product.ListPrice : (decimal)x.Product.Price) * x.Count)
            .Sum();
            //?
            ShoppingCartVM.OrderUserInfo.OrderStatus = OrderStatus.StatusPending;

            await _unitOfWork.OrderUserInfo.AddAsync(ShoppingCartVM.OrderUserInfo);
            await _unitOfWork.SaveAsync();

            foreach(var item in ShoppingCartVM.ShoppingCartList)
            {
                OrderDetailsProduct orderDetailsProduct = new()
                {
                    ProductId = item.ProductId,
                    OrderUserInfoId = ShoppingCartVM.OrderUserInfo.Id,
                    Count = item.Count,
                    Price = ShoppingCartVM.ShoppingCartList.Where(x => x.ProductId == item.ProductId).Select(x => x.Product.Price ?? x.Product.ListPrice).FirstOrDefault()
                //    Price = ShoppingCartVM.ShoppingCartList.Where(x => (x.ProductId == item.ProductId)).Select(x => ((x.Product.Price == null ? x.Product.ListPrice : (decimal)x.Product.Price)))
                };
                await _unitOfWork.OrderDetailsProduct.AddAsync(orderDetailsProduct);
                await _unitOfWork.SaveAsync();
            }

            return RedirectToAction("OrderSuccesfull", new {id = ShoppingCartVM.OrderUserInfo.Id});
        }
        [HttpGet]
        [ActionName("OrderSuccesfull")]
        public async Task<IActionResult> OrderCreatedSuccesfullAsync(int id)
        {
            return View(id);
        }
    }
}

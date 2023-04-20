using EBW.DataAccess;
using EBW.Models;
using EBW.Utility;
using Electronic_Bookstore_Web.PayPalApi;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PayPal.Api;
using System.Security.Claims;
using System.Web.Helpers;

namespace Electronic_Bookstore_Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _config;
        private readonly IPayPalService _payPalService;
        public CartController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor, IConfiguration config, IPayPalService payPalService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _config = config;
            _payPalService = payPalService;
        }
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ShoppingCartVM ShoppingCartVM = new()
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

           
            if(TempData.ContainsKey("CurrentInfoOrder"))
            {
                var Jsoncart = JsonConvert.DeserializeObject<ShoppingCartVM>(TempData["CurrentInfoOrder"].ToString());
                return View(Jsoncart);
            }
            else
            {
                ShoppingCartVM ShoppingCartVM = new()
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
        }

        [HttpPost]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPostAsync(ShoppingCartVM ShoppingCartVM)
        {
           
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                ShoppingCartVM.ShoppingCartList = await _unitOfWork.ShoppingCart.GetAllAsync(x => x.ApplicationUserId == userId, "Product");
                ShoppingCartVM.OrderUserInfo.OrderDate = DateTime.Now;
                ShoppingCartVM.OrderUserInfo.ApplicationUserId = userId;
                //? check 
                ShoppingCartVM.OrderUserInfo.ApplicationUser = await _userManager.GetUserAsync(User);

                ShoppingCartVM.OrderUserInfo.TotalOrderPrice = ShoppingCartVM.ShoppingCartList.Select(x => (
                x.Product.Price == null ? x.Product.ListPrice : (decimal)x.Product.Price) * x.Count)
                .Sum();
                if (ShoppingCartVM.OrderUserInfo.OrderStatus == null && ShoppingCartVM.OrderUserInfo.PaymentStatus == null)
                {
                    ShoppingCartVM.OrderUserInfo.OrderStatus = Status.StatusPending;
                    ShoppingCartVM.OrderUserInfo.PaymentStatus = Status.PaymentStatusPending;
                }
                await _unitOfWork.OrderUserInfo.AddAsync(ShoppingCartVM.OrderUserInfo);
                await _unitOfWork.SaveAsync();

                foreach (var item in ShoppingCartVM.ShoppingCartList)
                {
                    OrderDetailsProduct orderDetailsProduct = new()
                    {
                        ProductId = item.ProductId,
                        OrderUserInfoId = ShoppingCartVM.OrderUserInfo.Id,
                        Count = item.Count,
                        Price = ShoppingCartVM.ShoppingCartList.Where(x => x.ProductId == item.ProductId).Select(x => x.Product.Price ?? x.Product.ListPrice).FirstOrDefault()
                    };
                    await _unitOfWork.OrderDetailsProduct.AddAsync(orderDetailsProduct);
                    await _unitOfWork.SaveAsync();
                }

                var paypalurl = await _payPalService.PaymentAsync(HttpContext);

                var serializedObj = JsonConvert.SerializeObject(ShoppingCartVM);
                TempData["CurrentInfoOrder"] = serializedObj;
                return Redirect(paypalurl);
        }
        public async Task<IActionResult> Success()
        {
            ShoppingCartVM cart = new();
            if (TempData.ContainsKey("CurrentInfoOrder"))
            {
                cart = JsonConvert.DeserializeObject<ShoppingCartVM>(TempData["CurrentInfoOrder"].ToString());
            }

            var paymentResult = await _payPalService.ExecutePaymentAsync(HttpContext);
            if (paymentResult.state.ToLower() != "approved")
            {
                cart.OrderUserInfo.OrderStatus = Status.StatusInProcess;
                await _unitOfWork.OrderUserInfo.UpdateAsync(cart.OrderUserInfo);
                await _unitOfWork.SaveAsync();
                var serializedObj = JsonConvert.SerializeObject(cart);
                TempData["CurrentInfoOrder"] = serializedObj;
                return RedirectToAction("Summary");
            }
            else
            {
                cart.OrderUserInfo.OrderStatus = Status.StatusApproved;
                cart.OrderUserInfo.PaymentStatus = Status.PaymentStatusApproved;
                cart.OrderUserInfo.ShippingDate = DateTime.Now.AddDays(7);
                await _unitOfWork.OrderUserInfo.UpdateAsync(cart.OrderUserInfo);
                await _unitOfWork.SaveAsync();
                await ClearShoppingCartAsync(cart.OrderUserInfo.ApplicationUserId);
                return View("OrderSuccesfull");
            }
           
        }
        public async Task<IActionResult> Cancel()
        {
            return View("Summary");
        }
        public async Task ClearShoppingCartAsync(string userId)
        {
            var items = await _unitOfWork.ShoppingCart.GetAllAsync(x => x.ApplicationUserId == userId);
            await _unitOfWork.ShoppingCart.RemoveRangeAsync(items);
            await _unitOfWork.SaveAsync();
        }

        [HttpGet]
        [ActionName("HistoryOrder")]
        public async Task<IActionResult> HistoryOrderforUserAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            HistoryVM HistoryVM = new()
            {
                OrderUserInfoList = await _unitOfWork.OrderUserInfo.GetAllAsync(x=>x.ApplicationUserId == userId),
                OrderDetailsProductList = await _unitOfWork.OrderDetailsProduct.GetAllAsync(includeProp: new string[] { "OrderUserInfo", "Product" })
            };

            return View(HistoryVM);
        }
    }
}

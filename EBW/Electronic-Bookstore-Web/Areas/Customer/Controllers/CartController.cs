using EBW.DataAccess;
using EBW.Models;
using EBW.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Security.Claims;
using PayPal.Api;
using Electronic_Bookstore_Web.PayPalService;

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

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _config = config;
        }
        //public async Task<IActionResult> PaymentWithPayPal(string Cancel = null, string blogId = "", string PayerID = "", string guid = "")
        //{
        //    //getting the apiContext
        //    var ClientID = _config.GetValue<string>("PayPal:Key");
        //    var ClientSecret = _config.GetValue<string>("PayPal:Secret");
        //    var mode = _config.GetValue<string>("PayPal:mode");
        //    APIContext apiContext = PayPalConfiguration.GetAPIContext(ClientID, ClientSecret, mode);
        //    try
        //    {
        //        string payerId = PayerID;
        //        if(string.IsNullOrEmpty(payerId))
        //        {
        //            string baseURL = this.Request.Scheme + "://" + this.Request.Host + "/Cart/PaymentWithPayPal?";

        //            var giudd = Convert.ToString((new Random()).Next(100000));
        //            guid = giudd;

        //            var createPayment = this.CreatePayment(apiContext, baseURL + "guid=" + guid, blogId);
        //            var links = createPayment.links.GetEnumerator();
        //            string paypalRedirectUrl = null;
        //            while(links.MoveNext())
        //            {
        //                Links lnk = links.Current;
        //                if (lnk.rel.ToLower().Trim().Equals("approval_url"))
        //                {
        //                    paypalRedirectUrl= lnk.href;
        //                }
        //            }
        //            _contextAccessor.HttpContext.Session.SetString("payment", createPayment.id);
        //            return Redirect(paypalRedirectUrl);
        //        }
        //        else
        //        {
        //            var paymentId = _contextAccessor.HttpContext.Session.GetString("payment");
        //            var executedPayment = ExecutePayment(apiContext,payerId, paymentId as string);
        //            if(executedPayment.state.ToLower() != "approved")
        //            {
        //                return View("PaymentFailed");
        //            }

        //            var blogIds = executedPayment.transactions[0].item_list.items[0].sku;
        //            return View("PaymentSuccess");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("PaymentFailed");
        //    }

        //    return View("PaymentSuccess");
        //}

        //private PayPal.Api.Payment payment;

        //private Payment ExecutePayment(APIContext apiContext, string payerId,string paymentId)
        //{
        //    var paymentExecution = new PaymentExecution()
        //    {
        //        payer_id = payerId,
        //    };
        //    this.payment = new Payment()
        //    {
        //        id = paymentId
        //    };
        //    return this.payment.Execute(apiContext,paymentExecution);
        //}

        //private Payment CreatePayment(APIContext apiContext, string redirectURL, string blogId)
        //{
        //    var itemList = new ItemList() { items = new List<Item>() };
        //    itemList.items.Add(new Item()
        //    {
        //        name = "Item Details",
        //        currency = "USD",
        //        price = "1.00",
        //        quantity = "1",
        //        sku = "asd"
        //    });
        //    var payer = new Payer()
        //    {
        //        payment_method = "paypal"
        //    };
        //    var redirUrls = new RedirectUrls()
        //    {
        //        cancel_url = redirectURL + "&Cancel=true",
        //        return_url = redirectURL
        //    };
        //    var amount = new Amount()
        //    {
        //        currency = "USD",
        //        total = "1.00"
        //    };
        //    var transactionList = new List<Transaction>();
        //    transactionList.Add(new Transaction()
        //    {
        //        description = "Transaction description",
        //        invoice_number = Guid.NewGuid().ToString(),
        //        amount= amount,
        //        item_list= itemList
        //    });
        //    this.payment = new Payment()
        //    {
        //        intent = "sale",
        //        payer = payer,
        //        transactions = transactionList,
        //        redirect_urls = redirUrls
        //    };
        //    return this.payment.Create(apiContext);
        //}

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
            await ClearShoppingCartAsync(userId);
            return RedirectToAction("OrderSuccesfull", new {id = ShoppingCartVM.OrderUserInfo.Id});
        }

        [HttpGet]
        [ActionName("OrderSuccesfull")]
        public async Task<IActionResult> OrderCreatedSuccesfullAsync(int id)
        {
            return View(id);
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
            var orders = await _unitOfWork.OrderDetailsProduct.GetAllAsync(includeProp: new string[] { "OrderUserInfo", "Product"});
            orders = orders.Where(x => x.OrderUserInfo.ApplicationUserId == userId).ToList();
            return View(orders);
        }
    }
}

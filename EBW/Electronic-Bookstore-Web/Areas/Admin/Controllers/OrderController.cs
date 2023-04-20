using EBW.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Utility;
using Microsoft.AspNetCore.Authorization;
using EBW.Utility;
using EBW.Models;

namespace Electronic_Bookstore_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Role_Admin)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(int id)
        {
            OrderVM = new()
            {
                OrderUserInfo = await _unitOfWork.OrderUserInfo.GetFirstOrDefaultAsync(x => x.Id==id, includeProp: new string[] { "ApplicationUser" }),
                OrderDetail = await _unitOfWork.OrderDetailsProduct.GetAllAsync(x => x.OrderUserInfoId == id, includeProp: new string[] { "Product" })
            };

            return View(OrderVM);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrderUserInfo()
        {
            var orderUserInfoFromDb = await _unitOfWork.OrderUserInfo.GetFirstOrDefaultAsync(x=>x.Id == OrderVM.OrderUserInfo.Id);
            orderUserInfoFromDb.FirstName= OrderVM.OrderUserInfo.FirstName;
            orderUserInfoFromDb.LastName = OrderVM.OrderUserInfo.LastName;
            orderUserInfoFromDb.PhoneNumber= OrderVM.OrderUserInfo.PhoneNumber;
            orderUserInfoFromDb.City= OrderVM.OrderUserInfo.City;
            orderUserInfoFromDb.Carrier = OrderVM.OrderUserInfo.Carrier;
            orderUserInfoFromDb.BranchOffice= OrderVM.OrderUserInfo.BranchOffice;
            orderUserInfoFromDb.ShippingDate= OrderVM.OrderUserInfo.ShippingDate;
            if (!string.IsNullOrEmpty(OrderVM.OrderUserInfo.TrackingNumber))
            { orderUserInfoFromDb.TrackingNumber = OrderVM.OrderUserInfo.TrackingNumber; }
            if (!string.IsNullOrEmpty(OrderVM.OrderUserInfo.OrderStatus))
            { orderUserInfoFromDb.OrderStatus = OrderVM.OrderUserInfo.OrderStatus; }
           
            await _unitOfWork.OrderUserInfo.UpdateAsync(orderUserInfoFromDb);
            await _unitOfWork.SaveAsync();
            TempData["Success"] = "Order Details Updated";
            return RedirectToAction(nameof(Details), new { id = orderUserInfoFromDb.Id });
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll(string status)
        {
            var objOrderUserInfo = await _unitOfWork.OrderUserInfo.GetAllAsync(includeProp: new string[] { "ApplicationUser" });
            switch (status)
            {
                case "orderpending":
                    objOrderUserInfo = objOrderUserInfo.Where(x => x.OrderStatus == Status.StatusPending);
                    break;
                case "paymentpending":
                    objOrderUserInfo = objOrderUserInfo.Where(x => x.PaymentStatus == Status.PaymentStatusPending);
                    break;
                case "orderapproved":
                    objOrderUserInfo = objOrderUserInfo.Where(x => x.OrderStatus == Status.StatusApproved);
                    break;
                case "paymentapproved":
                    objOrderUserInfo = objOrderUserInfo.Where(x => x.PaymentStatus == Status.PaymentStatusApproved);
                    break;
                case "orderprocessing":
                    objOrderUserInfo = objOrderUserInfo.Where(x => x.OrderStatus == Status.StatusInProcess);
                    break;
                case "paymentrejected":
                    objOrderUserInfo = objOrderUserInfo.Where(x => x.PaymentStatus == Status.PaymentStatusRejected);
                    break;
                case "ordershipped":
                    objOrderUserInfo = objOrderUserInfo.Where(x => x.OrderStatus == Status.StatusShipped);
                    break;
                default:
                    break;
            }
            return Json(new { data = objOrderUserInfo });
        }
        #endregion

    }
}

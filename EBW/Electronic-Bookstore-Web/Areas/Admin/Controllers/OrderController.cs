using EBW.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Utility;
using Microsoft.AspNetCore.Authorization;
using EBW.Utility;

namespace Electronic_Bookstore_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Role_Admin)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
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

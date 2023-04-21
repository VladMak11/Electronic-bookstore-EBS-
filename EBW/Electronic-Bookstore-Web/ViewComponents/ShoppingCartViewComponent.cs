using EBW.DataAccess;
using EBW.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Electronic_Bookstore_Web.ViewComponents
{
    public class ShoppingCartViewComponent :ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(userClaim != null) 
            {
                if (HttpContext.Session.GetInt32(Status.SessionCart) == null)
                {
                    HttpContext.Session.SetInt32(Status.SessionCart, _unitOfWork.ShoppingCart.GetAllAsync(x => x.ApplicationUserId == userClaim.Value).GetAwaiter().GetResult().Count());
                }
                return View(HttpContext.Session.GetInt32(Status.SessionCart));
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
            
        }
    }
}

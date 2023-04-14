using EBW.DataAccess;
using EBW.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Electronic_Bookstore_Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ShoppingCartVM = new()
            {
                ShoppingCartList = await _unitOfWork.ShoppingCart.GetAllAsync(x => x.ApplicationUserId == userId,"Product"),
            };

            ShoppingCartVM.TotalPrice =  ShoppingCartVM.ShoppingCartList.Select(x => (
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

    }
}

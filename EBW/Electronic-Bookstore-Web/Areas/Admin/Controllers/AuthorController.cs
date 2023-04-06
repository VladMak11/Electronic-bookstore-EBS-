using EBW.DataAccess;
using EBW.Models;
using EBW.Models.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Electronic_Bookstore_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AuthorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Author> objAuthorList = await _unitOfWork.Author.GetAllAsync();
            return View(objAuthorList);
        }
        //GET
        [HttpGet, ActionName("Upsert")]
        public async Task<IActionResult> UpsertGetAsync(int? id)
        {
            Author author = new();

            if (id == null || id == 0)
            {
                return View(author);
            }
            else
            {
                author = await _unitOfWork.Author.GetFirstOrDefaultAsync(x => x.Id == id);
                return View(author);
            }
        }

        [HttpPost, ActionName("Upsert")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertPostAsync(Author obj)
        {
            var localvalidator = new AuthorValidator();
            var result = localvalidator.Validate(obj);

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    await _unitOfWork.Author.AddAsync(obj);
                    TempData["success"] = "Author added";
                }
                else
                {
                    await _unitOfWork.Author.UpdateAsync(obj);
                }

                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(obj);
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authortList = await _unitOfWork.Author.GetAllAsync();
            return Json(new { data = authortList });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _unitOfWork.Author.GetFirstOrDefaultAsync(u => u.Id == id);

            if (author == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            
            await _unitOfWork.Author.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}

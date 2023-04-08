
using EBW.DataAccess;
using EBW.Models;
using EBW.Models.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_Bookstore_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> objCategoryList = await _unitOfWork.Category.GetAllAsync();
            return View(objCategoryList);
        }

        //GET
        [HttpGet, ActionName("Upsert")]
        public async Task<IActionResult> UpsertGetAsync(int? id)
        {
            Category category = new();

            if (id == null || id == 0)
            {
                return View(category);
            }
            else
            {
                category = await _unitOfWork.Category.GetFirstOrDefaultAsync(x => x.Id == id);
                return View(category);
            }
        }

        [HttpPost, ActionName("Upsert")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertPostAsync(Category obj)
        {
            var localvalidator = new CategoryValidator();
            var result = localvalidator.Validate(obj);

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    await _unitOfWork.Category.AddAsync(obj);
                    TempData["success"] = "Category added";
                }
                else
                {
                    await _unitOfWork.Category.UpdateAsync(obj);
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
            var categoryList = await _unitOfWork.Category.GetAllAsync();
            return Json(new { data = categoryList });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _unitOfWork.Category.GetFirstOrDefaultAsync(u => u.Id == id);

            if (category == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            await _unitOfWork.Category.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}

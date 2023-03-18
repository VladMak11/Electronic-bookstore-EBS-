
using EBW.DataAccess;
using EBW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Electronic_Bookstore_Web.Controllers
{
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<CoverType> objCoverTypeList = await _unitOfWork.CoverType.GetAllAsync();
            return View(objCoverTypeList);
        }

        //GET
        [HttpGet, ActionName("Upsert")]
        public async Task<IActionResult> UpsertGetAsync(int? id)
        {
            CoverType covertype = new();

            if (id == null || id == 0)
            {
                return View(covertype);
            }
            else
            {
                covertype = await _unitOfWork.CoverType.GetFirstOrDefaultAsync(x => x.Id == id);
                return View(covertype);
            }
        }

        [HttpPost, ActionName("Upsert")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertPostAsync(CoverType obj)
        {
            var localvalidator = new CoverTypeValidator();
            var result = localvalidator.Validate(obj);

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    await _unitOfWork.CoverType.AddAsync(obj);
                    TempData["success"] = "Cover Type added";
                }
                else
                {
                    await _unitOfWork.CoverType.UpdateAsync(obj);
                }

                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(obj);
        }
        //GET
        [HttpGet, ActionName("Delete")]
        public async Task<IActionResult> DeleteGetAsync(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var covertype = await _unitOfWork.CoverType.GetFirstOrDefaultAsync(u => u.Id == id);

            if (covertype == null)
            {
                return NotFound();
            }

            return View(covertype);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePostAsync(int id)
        {
            var covertype = await _unitOfWork.CoverType.GetFirstOrDefaultAsync(u => u.Id == id);

            if (covertype == null)
            {
                return NotFound();
            }
            await _unitOfWork.CoverType.RemoveAsync(id);
            TempData["success"] = "Cover Type deleted";
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

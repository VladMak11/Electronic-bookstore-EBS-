using Electronic_Bookstore_Web.Data;
using Electronic_Bookstore_Web.Models;
using Electronic_Bookstore_Web.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Electronic_Bookstore_Web.Controllers
{
    public class CoverTypeController : Controller
    {
        private readonly ApplicationDBContext _db;
        public CoverTypeController(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<CoverType> objCoverTypeList = await _db.CoverTypes.ToListAsync();
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
                covertype = await _db.CoverTypes.FirstOrDefaultAsync(u => u.Id == id);
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
                    await _db.CoverTypes.AddAsync(obj);
                    TempData["success"] = "Cover Type added";
                }
                else
                {
                    _db.CoverTypes.Update(obj);
                }

                await _db.SaveChangesAsync();
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

            var covertype = await _db.CoverTypes.FirstOrDefaultAsync(u => u.Id == id);

            if (covertype == null)
            {
                return NotFound();
            }

            return View(covertype);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePostAsync(int? id)
        {
            var covertype = await _db.CoverTypes.FirstOrDefaultAsync(u => u.Id == id);

            if (covertype == null)
            {
                return NotFound();
            }
            _db.CoverTypes.Remove(covertype);
            TempData["success"] = "Cover Type deleted";
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

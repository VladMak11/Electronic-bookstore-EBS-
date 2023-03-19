using EBW.DataAccess;
using EBW.Models;
using EBW.Models.Validators;
using Microsoft.AspNetCore.Mvc;

namespace Electronic_Bookstore_Web.Controllers
{
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

        //GET
        [HttpGet, ActionName("Delete")]
        public async Task<IActionResult> DeleteGetAsync(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var author = await _unitOfWork.Author.GetFirstOrDefaultAsync(u => u.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePostAsync(int id)
        {
            var author = await _unitOfWork.Author.GetFirstOrDefaultAsync(u => u.Id == id);

            if (author == null)
            {
                return NotFound();
            }
            await _unitOfWork.Author.RemoveAsync(id);
            TempData["success"] = "Author deleted";
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

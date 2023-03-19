using EBW.DataAccess;
using EBW.Models.Validators;
using EBW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EBW.DataAccess.ViewModels;

namespace Electronic_Bookstore_Web.Controllers
{
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> objProductList = await _unitOfWork.Product.GetAllAsync();
            return View(objProductList);
        }
        //GET
        [HttpGet, ActionName("Upsert")]
        public async Task<IActionResult> UpsertGetAsync(int? id)
        {
            var categories = await _unitOfWork.Category.GetAllAsync();
            var covertypes = await _unitOfWork.CoverType.GetAllAsync();
            var authors = await _unitOfWork.Author.GetAllAsync();

            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = categories.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                CoverTypeList = covertypes.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                AuthorList = authors.Select(x => new SelectListItem { Text = x.FullName, Value = x.Id.ToString() })
            };

            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = await _unitOfWork.Product.GetFirstOrDefaultAsync(x => x.Id == id);
                return View(productVM);
            }
        }

        [HttpPost, ActionName("Upsert")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertPostAsync(ProductVM obj, IFormFile file)
        {
            //var localvalidator = new AuthorValidator();
            //var result = localvalidator.Validate(obj);

            //foreach (var error in result.Errors)
            //{
            //    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            //}
            if (ModelState.IsValid)
            {
                if (obj.Product.Id == 0)
                {
                    await _unitOfWork.Product.AddAsync(obj.Product);
                    TempData["success"] = "Product added";
                }
                else
                {
                    await _unitOfWork.Product.UpdateAsync(obj.Product);
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

            var product = await _unitOfWork.Product.GetFirstOrDefaultAsync(u => u.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePostAsync(int id)
        {
            var product = await _unitOfWork.Product.GetFirstOrDefaultAsync(u => u.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            await _unitOfWork.Product.RemoveAsync(id);
            TempData["success"] = "Product deleted";
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

    }

}

using EBW.DataAccess;
using EBW.Models.Validators;
using EBW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EBW.DataAccess.ViewModels;
using EBW.DataAcces;

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

        public IActionResult Index()
        {
            return View();
        }
        //GET
        [HttpGet, ActionName("Upsert")]
        public async Task<IActionResult> UpsertGetAsync(int? id)
        {
            var categories = await _unitOfWork.Category.GetAllAsync();
            var covertypes = await _unitOfWork.CoverType.GetAllAsync();
            var authors = await _unitOfWork.Author.GetAllAsync();
            ProductVM productVM = null;
            //productVM = new ProductVM(categories, covertypes, authors)
            //{
            //    Product = new(),
            //    //CategoryList = categories.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
            //    //CoverTypeList = covertypes.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
            //    //AuthorList = authors.Select(x => new SelectListItem { Text = x.FullName, Value = x.Id.ToString() })
            //};

            if (id == null || id == 0)
            {
                //
                productVM = new ProductVM(categories, covertypes, authors);
                return View(productVM);
            }
            else
            {
                var product = await _unitOfWork.Product.GetFirstOrDefaultAsync(x => x.Id == id);
                productVM = new ProductVM(categories, covertypes, authors)
                {
                    Product = product
                };
                //productVM.Product = await _unitOfWork.Product.GetFirstOrDefaultAsync(x => x.Id == id);
                return View(productVM);
            }
        }

        [HttpPost, ActionName("Upsert")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertPostAsync(ProductVM obj, IFormFile? file)
        {
            var localvalidator = new ProductValidator();
            var result = localvalidator.Validate(obj);

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath,@"images\products");  
                    var extension = Path.GetExtension(file.FileName);
                    if(obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads,fileName+extension),FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }
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
            else
            {
                var categories = await _unitOfWork.Category.GetAllAsync();
                var covertypes = await _unitOfWork.CoverType.GetAllAsync();
                var authors = await _unitOfWork.Author.GetAllAsync();
                obj.CategoryList = categories.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                obj.CoverTypeList = covertypes.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
                obj.AuthorList = authors.Select(x => new SelectListItem { Text = x.FullName, Value = x.Id.ToString() });
            }

            return View(obj);
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productList = await _unitOfWork.Product.GetAllAsync("Author","Category","CoverType");
            return Json(new { data = productList });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Product.GetFirstOrDefaultAsync(u => u.Id == id);

            if (product == null)
            {
                return Json(new { success = false, message = "Error while deleting"});
            }
            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            await _unitOfWork.Product.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion

    }
}

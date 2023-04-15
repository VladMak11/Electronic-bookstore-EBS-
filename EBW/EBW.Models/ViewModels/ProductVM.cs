using EBW.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace EBW.DataAccess.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CoverTypeList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> AuthorList { get; set; }
        public ProductVM() { }
        public ProductVM(IEnumerable<Category> categories, IEnumerable<CoverType> covertypes, IEnumerable<Author> authors)
        {
            CategoryList = categories.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            CoverTypeList = covertypes.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            AuthorList = authors.Select(x => new SelectListItem { Text = x.FullName, Value = x.Id.ToString() });
            Product = new Product();
        }
    }
}

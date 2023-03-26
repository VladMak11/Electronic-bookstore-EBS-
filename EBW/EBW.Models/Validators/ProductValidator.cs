using EBW.DataAccess.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EBW.Models.Validators
{
    public class ProductValidator : AbstractValidator<ProductVM>
    {
        public ProductValidator()
        {
            RuleFor(product => product.Product.Title)
               .NotEmpty().WithMessage("The field can't be empty")
               .Length(3, 25).WithMessage("The Product name must be between 3 and 25 characters");
            RuleFor(product => product.Product.Description)
                .MaximumLength(4000).WithMessage("Description can't be more than 4000 characters");
            RuleFor(product => product.Product.ISBN)
                .Matches(@"^\d{17}$").WithMessage("ISBN must be 17 characters");
            RuleFor(product => product.Product.ListPrice)
                .NotEmpty().WithMessage("The field can't be empty");
            RuleFor(product => product.Product.CategoryId)
                 .NotEmpty().WithMessage("Select category");
            RuleFor(product => product.Product.AuthorId)
                 .NotEmpty().WithMessage("Select author");
            RuleFor(product => product.Product.CoverTypeId)
                .NotEmpty().WithMessage("Select cover type");
        }
    }
}

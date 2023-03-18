using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBW.Models.Validators
{
    public  class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(category => category.Name)
               .NotEmpty().WithMessage("The field can't be empty")
               .Length(3, 20).WithMessage("The Category name must be between 3 and 20 characters");
        }
    }
}

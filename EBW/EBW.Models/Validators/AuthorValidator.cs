using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBW.Models.Validators
{
    public class AuthorValidator : AbstractValidator<Author>
    {
        public AuthorValidator()
        {
            RuleFor(author => author.FullName)
               .NotEmpty().WithMessage("The field can't be empty")
               .Length(3, 20).WithMessage("The Category name must be between 2 and 20 characters")
               .Matches("^(?=.{2,30}$)[A-Z][a-z]+\\s[A-Z]\\.[A-Z]\\.$").WithMessage("The Author name must be in the format 'Shevchenko T.G.'");
        }
    }
}

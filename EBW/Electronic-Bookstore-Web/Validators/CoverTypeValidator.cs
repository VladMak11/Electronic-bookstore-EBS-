using Electronic_Bookstore_Web.Models;
using FluentValidation;

namespace Electronic_Bookstore_Web.Validators
{
    public class CoverTypeValidator : AbstractValidator<CoverType>
    {
        public CoverTypeValidator()
        {
            RuleFor(covertype => covertype.Name)
                .Length(3, 20).WithMessage("Min length, it's 3 characters.");
        }
    }
}

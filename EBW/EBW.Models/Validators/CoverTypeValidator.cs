
using FluentValidation;

namespace EBW.Models
{
    public class CoverTypeValidator : AbstractValidator<CoverType>
    {
        public CoverTypeValidator()
        {
            RuleFor(covertype => covertype.Name)
               .NotEmpty().WithMessage("The field can't be empty")
               .Length(3, 20).WithMessage("The Category name must be between 3 and 20 characters");
        }
    }
}

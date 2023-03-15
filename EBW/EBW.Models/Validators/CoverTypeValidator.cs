
using FluentValidation;

namespace EBW.Models
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

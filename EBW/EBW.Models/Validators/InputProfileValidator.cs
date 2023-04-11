using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBW.Models
{
    public class InputProfileValidator : AbstractValidator<InputProfile>
    {
        public InputProfileValidator()
        {
            RuleFor(model => model.LastName)
               .NotEmpty().WithMessage("The field can't be empty")
                   .Length(2, 25).WithMessage("The Last Name must be between 2 and 25 characters");

            RuleFor(model => model.FirstName)
               .NotEmpty().WithMessage("The field can't be empty")
                   .Length(2, 25).WithMessage("The First Name must be between 2 and 25 characters");

            RuleFor(model => model.PhoneNumber)
               .NotEmpty().WithMessage("The field can't be empty")
                   .Matches(@"^\+380\d{9}$").WithMessage("Invalid phone number format.The phone number must be in the format + 380XXXXXXXXX."); ;
        }
    }
}

using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBW.Models.Validators
{
    public class InputModelforRegistrationValidator : AbstractValidator<InputModelforRegistration>
    {
        public InputModelforRegistrationValidator( )
        {

            RuleFor(model => model.Email)
                .NotEmpty().WithMessage("The field can't be empty")
                    .Matches(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").WithMessage("The E-mail name must be in the right email format.");

            RuleFor(model => model.Password)
                .NotEmpty().WithMessage("The field can't be empty");

            RuleFor(model => model.ConfirmPassword)
                .NotEmpty().WithMessage("The field can't be empty");

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

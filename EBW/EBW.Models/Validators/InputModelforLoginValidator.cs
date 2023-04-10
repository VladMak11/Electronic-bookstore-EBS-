using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBW.Models
{
    public class InputModelforLoginValidator : AbstractValidator<InputModelforLogin>
    {
        public InputModelforLoginValidator()
        {
            RuleFor(model => model.Email)
               .NotEmpty().WithMessage("The field can't be empty")
                   .Matches(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").WithMessage("The E-mail name must be in the right email format.");
            RuleFor(model => model.Password)
                .NotEmpty().WithMessage("The field can't be empty");
        }
    }
}

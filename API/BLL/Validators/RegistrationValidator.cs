using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.BLL.Validators
{
    public class RegistrationValidator : AbstractValidator<API.BLL.Services.AccessControl.Registration.Models.InModel>
    {
        public RegistrationValidator()
        {
            RuleFor(model => model.UserName)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3);
            RuleFor(model => model.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress();
            RuleFor(model => model.PasswordHash)
                .NotNull()
                .NotEmpty();
        }
    }
}

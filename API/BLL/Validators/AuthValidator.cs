using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.BLL.Validators
{
    public class AuthValidator : AbstractValidator<API.BLL.Services.AccessControl.Authentication.Models.InModel>
    {
        public AuthValidator()
        {
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

using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.BLL.Validators
{
    public class ResetPasswordValidator : AbstractValidator<API.BLL.Services.Users.ResetPassword.Models.InModel>
    {
        public ResetPasswordValidator()
        {
            RuleFor(model => model.IdUser )
                .NotNull()
                .NotEmpty();
            RuleFor(model => model.NewPasswordHash)
                .NotNull()
                .NotEmpty();
            RuleFor(model => model.OldPasswordHash)
                .NotNull()
                .NotEmpty();
        }
    }
}

using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.BLL.Validators
{
    public class ChangeProfileValidator : AbstractValidator<API.BLL.Services.Users.ChangeProfile.Models.InModel>
    {
        public ChangeProfileValidator()
        {
            RuleFor(model => model.Username)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3);
            RuleFor(model => model.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress();
            RuleFor(model => model.UserId)
                .NotNull()
                .NotEmpty();
        }


    }
}

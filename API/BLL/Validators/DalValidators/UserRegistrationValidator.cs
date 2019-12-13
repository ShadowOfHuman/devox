using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.BLL.Validators.DalValidators
{
    public class UserRegistrationValidator : AbstractValidator<API.DAL.Models.User>
    {
        public UserRegistrationValidator()
        {
            RuleFor(model => model.Username)
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

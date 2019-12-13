using FluentValidation;

namespace API.BLL.Validators.DalValidators
{
    public class UserAuthValidator : AbstractValidator<API.DAL.Models.User>
    {
        public UserAuthValidator()
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

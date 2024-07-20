using DTO.Auth;
using FluentValidation;

namespace BusinessLayer.Validators.User;

public class UserLoginRequestValidator : AbstractValidator<UserLoginRequest>
{
    public UserLoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).MinimumLength(6).NotEmpty();
    }
}

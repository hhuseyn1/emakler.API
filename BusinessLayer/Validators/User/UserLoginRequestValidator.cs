using DTO.Auth;
using FluentValidation;

namespace BusinessLayer.Validators.User;

public class UserLoginRequestValidator : AbstractValidator<UserLoginRequest>
{
    public UserLoginRequestValidator()
    {
        RuleFor(x => x.ContactNumber)
                          .NotEmpty().WithMessage("Contact number is required,")
                          .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid contact number!");
        RuleFor(x => x.Password).MinimumLength(6).NotEmpty();
    }
}

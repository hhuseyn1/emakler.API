using DTO.Auth;
using FluentValidation;

namespace BusinessLayer.Validators.User;

public class UserRegisterRequestValidator : AbstractValidator<UserRegisterRequest>
{
    public UserRegisterRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname is required.");

        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.ContactNumber)
                   .NotEmpty().WithMessage("Contact number is required,")
                   .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid contact number!");

        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
    }
}

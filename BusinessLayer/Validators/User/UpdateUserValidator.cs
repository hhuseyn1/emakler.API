using DTO.User;
using FluentValidation;

namespace BusinessLayer.Validators.User;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address");

        RuleFor(x => x.ContactNumber)
            .NotEmpty().WithMessage("Contact number is required")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid contact number");
    }
}

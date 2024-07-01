using FluentValidation;
using DTO.Building;

namespace BusinessLayer.Validators;

public class BuildingFilterValidator : AbstractValidator<BuildingFilter>
{
    public BuildingFilterValidator()
    {
        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0).When(x => x.MinPrice.HasValue)
            .WithMessage("Minimum Price must be a positive value.");

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(x => x.MinPrice)
            .When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue)
            .WithMessage("Maximum Price must be greater than or equal to Minimum Price.");

        RuleFor(x => x.Address)
            .MaximumLength(100)
            .WithMessage("Address cannot exceed 100 characters.");

        RuleFor(x => x.Metro)
            .MaximumLength(50)
            .WithMessage("Metro cannot exceed 50 characters.");

        RuleFor(x => x.City)
            .MaximumLength(50)
            .WithMessage("City cannot exceed 50 characters.");

        RuleFor(x => x.Village)
            .MaximumLength(50)
            .WithMessage("Village cannot exceed 50 characters.");

        RuleFor(x => x.District)
            .MaximumLength(50)
            .WithMessage("District cannot exceed 50 characters.");

        RuleFor(x => x.RoomCount)
            .GreaterThanOrEqualTo(1).When(x => x.RoomCount.HasValue)
            .LessThanOrEqualTo(10).When(x => x.RoomCount.HasValue)
            .WithMessage("Room Count must be between 1 and 10.");

        RuleFor(x => x.Area)
            .GreaterThanOrEqualTo(1).When(x => x.Area.HasValue)
            .LessThanOrEqualTo(10000).When(x => x.Area.HasValue)
            .WithMessage("Area must be between 1 and 10000.");

        RuleFor(x => x.AdType)
            .Must(x => x == "Sale" || x == "Rent")
            .WithMessage("AdType must be 'Sale' or 'Rent'.");

        RuleFor(x => x.SellerType)
            .Must(x => x == "Owner" || x == "Agency")
            .WithMessage("SellerType must be 'Owner' or 'Agency'.");
    }
}

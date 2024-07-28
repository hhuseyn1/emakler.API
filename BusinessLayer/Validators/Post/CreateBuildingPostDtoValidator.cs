using DTO.BuildingPost;
using FluentValidation;

namespace BusinessLayer.Validators.Post;

public class CreateBuildingPostDtoValidator : AbstractValidator<BuildingPostDto>
{
    public CreateBuildingPostDtoValidator()
    {
        RuleFor(dto => dto.Building.Metro).NotEmpty().WithMessage("Metro is required.");
        RuleFor(dto => dto.Building.City).NotEmpty().WithMessage("City is required.");
        RuleFor(dto => dto.Building.Village).NotEmpty().WithMessage("Village is required.");
        RuleFor(dto => dto.Building.District).NotEmpty().WithMessage("District is required.");
        RuleFor(dto => dto.Building.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
        RuleFor(dto => dto.Building.RoomCount).GreaterThan(0).WithMessage("RoomCount must be greater than zero.");
        RuleFor(dto => dto.Building.Area).GreaterThan(0).WithMessage("Area must be greater than zero.");
    }
}

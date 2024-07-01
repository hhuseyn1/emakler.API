using System.ComponentModel.DataAnnotations;

namespace DTO.Building;

public class BuildingCreateRequest
{
    [Required(ErrorMessage = "Metro is required.")]
    [StringLength(100, ErrorMessage = "Metro name cannot be longer than 100 characters.")]
    public string Metro { get; set; }

    [Required(ErrorMessage = "City is required.")]
    [StringLength(100, ErrorMessage = "City name cannot be longer than 100 characters.")]
    public string City { get; set; }

    [StringLength(100, ErrorMessage = "Village name cannot be longer than 100 characters.")]
    public string Village { get; set; }

    [Required(ErrorMessage = "District is required.")]
    [StringLength(100, ErrorMessage = "District name cannot be longer than 100 characters.")]
    public string District { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative value.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Room count is required.")]
    [Range(1, 50, ErrorMessage = "Room count must be between 1 and 50.")]
    public int RoomCount { get; set; }

    [Required(ErrorMessage = "Area is required.")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Area must be a positive value.")]
    public decimal Area { get; set; }

    [Required(ErrorMessage = "Ad type is required.")]
    [StringLength(50, ErrorMessage = "Ad type cannot be longer than 50 characters.")]
    public string AdType { get; set; } // E.g., Sale, Rent

    [Required(ErrorMessage = "Seller type is required.")]
    [StringLength(50, ErrorMessage = "Seller type cannot be longer than 50 characters.")]
    public string SellerType { get; set; } // E.g., Owner, Agency

    [Required(ErrorMessage = "IsActive status is required.")]
    public bool IsActive { get; set; }
}

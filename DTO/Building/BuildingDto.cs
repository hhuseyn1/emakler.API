using System.ComponentModel.DataAnnotations;

namespace DTO.Building;

public class BuildingDTO
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Metro is required.")]
    [StringLength(50, ErrorMessage = "Metro cannot exceed 50 characters.")]
    public string Metro { get; set; }

    [Required(ErrorMessage = "City is required.")]
    [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
    public string City { get; set; }

    [StringLength(50, ErrorMessage = "Village cannot exceed 50 characters.")]
    public string Village { get; set; }

    [Required(ErrorMessage = "District is required.")]
    [StringLength(50, ErrorMessage = "District cannot exceed 50 characters.")]
    public string District { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "RoomCount is required.")]
    [Range(1, 10, ErrorMessage = "RoomCount must be between 1 and 10.")]
    public int RoomCount { get; set; }

    [Required(ErrorMessage = "Area is required.")]
    [Range(1, 10000, ErrorMessage = "Area must be between 1 and 10000.")]
    public decimal Area { get; set; }

    [Required(ErrorMessage = "AdType is required.")]
    [StringLength(10, ErrorMessage = "AdType cannot exceed 10 characters.")]
    public string AdType { get; set; }

    [Required(ErrorMessage = "SellerType is required.")]
    [StringLength(10, ErrorMessage = "SellerType cannot exceed 10 characters.")]
    public string SellerType { get; set; }

}

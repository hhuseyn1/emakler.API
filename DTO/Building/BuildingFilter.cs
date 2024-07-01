using System.ComponentModel.DataAnnotations;

namespace DTO.Building;

public class BuildingFilter
{
    [Range(0, double.MaxValue, ErrorMessage = "MinPrice must be a non-negative value.")]
    public decimal? MinPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "MaxPrice must be a non-negative value.")]
    public decimal? MaxPrice { get; set; }

    [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
    public string Address { get; set; }

    [StringLength(50, ErrorMessage = "Metro cannot exceed 50 characters.")]
    public string Metro { get; set; }

    [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
    public string City { get; set; }

    [StringLength(50, ErrorMessage = "Village cannot exceed 50 characters.")]
    public string Village { get; set; }

    [StringLength(50, ErrorMessage = "District cannot exceed 50 characters.")]
    public string District { get; set; }

    [Range(1, 10, ErrorMessage = "RoomCount must be between 1 and 10.")]
    public int? RoomCount { get; set; }

    [Range(1, 10000, ErrorMessage = "Area must be between 1 and 10000.")]
    public decimal? Area { get; set; }

    [AllowedValues("Sale", "Rent", ErrorMessage = "AdType must be either 'Sale' or 'Rent'.")]
    public string AdType { get; set; }

    [AllowedValues("Owner", "Agency", ErrorMessage = "SellerType must be either 'Owner' or 'Agency'.")]
    public string SellerType { get; set; }
}

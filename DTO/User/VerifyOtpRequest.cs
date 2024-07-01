using System.ComponentModel.DataAnnotations;

namespace DTO.User;

public class VerifyOtpRequest
{
    [Required]
    [Phone]
    public string ContactNumber { get; set; }

    [Required]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP code must be exactly 6 digits.")]
    public string OtpCode { get; set; }
}

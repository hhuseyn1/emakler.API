using System.ComponentModel.DataAnnotations;

namespace DTO.User;

public class UserRegistration
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [Phone]
    public string ContactNumber { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}

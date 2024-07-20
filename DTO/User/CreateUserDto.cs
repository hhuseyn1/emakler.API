using System.ComponentModel.DataAnnotations;

namespace DTO.User;

public class CreateUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string ContactNumber { get; set; }
}

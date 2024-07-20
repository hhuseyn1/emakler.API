using System.ComponentModel.DataAnnotations;

namespace DTO.User;

public class UpdateUserDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string ContactNumber { get; set; }
}

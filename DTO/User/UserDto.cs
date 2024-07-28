namespace DTO.User;

public class UserDto
{
    public string Email { get; set; } = null!;
    public string ContactNumber { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsVerified { get; set; } = false;
}

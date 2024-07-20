namespace DTO.User;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string ContactNumber { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? OtpCode { get; set; }
    public DateTime OtpCreatedTime { get; set; } = DateTime.UtcNow;
    public bool IsVerified { get; set; } = false;
}

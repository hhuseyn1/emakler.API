namespace DTO.User;

public class UpdateUserDto
{
    public string UserMail { get; set; } = null!;
    public string? ContactNumber { get; set; }
    public string? OtpCode { get; set; }
    public DateTime? OtpCreatedTime { get; set; }
    public bool? IsValidate { get; set; }
    public string? Password { get; set; }
}



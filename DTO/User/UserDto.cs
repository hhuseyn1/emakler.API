namespace DTO.User;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserMail { get; set; } = null!;
    public string ContactNumber { get; set; } = null!;
    public bool IsEmailConfirmed { get; set; }
    public DateTime CreatedDate { get; set; }
}

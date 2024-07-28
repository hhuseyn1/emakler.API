namespace DTO.Auth;

public class UserRegisterRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string ContactNumber { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}

namespace DTO.User;

public class ResetPasswordModel
{
    public string PhoneNumber { get; set; }
    public string OtpCode {  get; set; }
    public string NewPassword { get; set; }
}

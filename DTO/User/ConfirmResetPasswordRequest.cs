namespace DTO.User;

public class ConfirmResetPasswordRequest
{
    public string PhoneNumber { get; set; }
    public string OtpCode {  get; set; }
    public string NewPassword { get; set; }
}

namespace DTO.User;

public class UserVerificationRequest
{
    public string ContactNumber { get; set; }
    public string OtpCode { get; set; }
}

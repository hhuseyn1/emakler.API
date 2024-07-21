namespace BusinessLayer.Interfaces.OtpService;

public interface IOtpService
{
    Task<string> SendOtpAsync(string phoneNumber);
    Task<bool> VerifyOtpAsync(string phoneNumber, string otpCode);

}

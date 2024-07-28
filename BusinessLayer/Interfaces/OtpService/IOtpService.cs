namespace BusinessLayer.Interfaces.OtpService;

public interface IOtpService
{
    Task SendOtpAsync(string phoneNumber);
    Task<bool> VerifyOtpAsync(string phoneNumber, string otp);
}

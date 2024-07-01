namespace BusinessLayer.Interfaces.UserServices;

public interface IOtpService
{
    Task<bool> SendOtpAsync(string contactNumber);
    Task<bool> VerifyOtpAsync(string contactNumber, string otpCode);
}

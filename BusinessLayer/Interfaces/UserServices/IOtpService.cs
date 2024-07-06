using DTO.User;

namespace BusinessLayer.Interfaces.UserServices;

public interface IOtpService
{
    Task<bool> SendOtpAsync(SendOtpRequest request);
    Task<bool> VerifyOtpAsync(VerifyOtpRequest request);
}

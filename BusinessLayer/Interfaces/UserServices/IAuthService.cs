using DTO.User;

namespace BusinessLayer.Interfaces.UserServices;

public interface IAuthService
{
    Task<string> Login(UserLoginRequest request);
    Task<bool> VerifyOtp(string contactNumber, string otpCode);
}

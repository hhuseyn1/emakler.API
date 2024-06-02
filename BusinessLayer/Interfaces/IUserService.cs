using DTO.User;

namespace BusinessLayer.Interfaces;

public interface IUserService
{
    Task RegisterUser(UserRegistration userRegistration);
    Task<bool> ValidateUser(string userMail, string password);
    Task SendOtp(string contactNumber);
    Task<bool> VerifyOtp(string contactNumber, string otpCode);
    Task UpdateUser(Guid userId, UserRegistration userRegistration);
    Task DeleteUser(Guid userId);
    Task<bool> RequestPasswordReset(string phoneNumber);
    Task<bool> ConfirmPasswordReset(ConfirmResetPasswordRequest request);
}

using DTO.User;

namespace BusinessLayer.Interfaces;

public interface IUserService
{
    Task RegisterUser(AddUserDto addUserDto); 
    Task UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto); 
    Task DeleteUserAsync(Guid userId); 
    Task<UserDto> GetUserByIdAsync(Guid userId);
    Task<UserDto> GetUserByMailAsync(string userMail); 
    Task<UserDto> GetUserByContactNumberAsync(string contactNumber);
    Task<bool> ValidateUserAsync(string userMail, string password); 
    Task<bool> SendOtpAsync(SendOtpRequest request); 
    Task<bool> VerifyOtpAsync(VerifyOtpRequest request); 
    Task ConfirmEmailAsync(Guid userId);
    Task ChangePasswordAsync(ChangePasswordRequest request);
}

using DTO.User;
using EntityLayer.Entities;

namespace BusinessLayer.Interfaces;

public interface IUserService
{
    Task RegisterUser(UserRegistration userRegistration);
    Task UpdateUser(Guid userId, UserRegistration userRegistration);
    Task DeleteUser(Guid userId);
    Task<User> GetUserByMailAsync(string userMail);
    Task<User> GetUserByContactNumberAsync(string contactNumber);
    Task<bool> ValidateUser(string userMail, string password);
    Task ConfirmEmail(Guid userId);
    Task ChangePassword(ChangePasswordRequest request);

}

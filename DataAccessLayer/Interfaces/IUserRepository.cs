using DTO.User;
using EntityLayer.Entities;

namespace DataAccessLayer.Interfaces;

public interface IUserRepository
{
    Task AddUserAsync(AddUserDto addUserDto);
    Task<User> GetUserByUsernameAsync(string userMail);
    Task<User> GetUserByIdAsync(Guid userId);
    Task<User> GetUserByContactNumberAsync(string contactNumber);
    Task UpdateUserAsync(UpdateUserDto updateUserDto);
    Task DeleteUserAsync(Guid userId);
}

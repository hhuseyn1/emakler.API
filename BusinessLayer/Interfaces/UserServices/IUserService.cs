using DTO.User;
using EntityLayer.Entities;

namespace BusinessLayer.Interfaces.UserServices;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(Guid userId);
    Task<User> GetUserByContactNumberAsync(string contactNumber);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> CreateUserAsync(UserDto userDto);
    Task<User> UpdateUserbyIdAsync(Guid userId, UserDto userDto);
    Task<bool> DeleteUserbyIdAsync(Guid userId);
}

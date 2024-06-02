using EntityLayer.Entities;

namespace DataAccessLayer.Interfaces;

public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<User> GetUserByUsernameAsync(string userMail);
    Task<User> GetUserByIdAsync(Guid userId);
    Task<User> GetUserByContactNumberAsync(string contactNumber);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid userId);
}

using EntityLayer.Entities;

namespace DataAccessLayer.Interfaces;

public interface IUserRepository
{
    Task AddUser(User user);
    Task<User> GetUserByUsername(string userMail);
    Task<User> GetUserById(Guid userId);
    Task<User> GetUserByContactNumber(string contactNumber);
    Task UpdateUser(User user);
    Task DeleteUser(Guid userId);
}

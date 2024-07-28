using EntityLayer.Entities;
using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(Guid userId);
    Task<User> GetUserByPredicateAsync(Expression<Func<User, bool>> predicate);
    Task AddUserAsync(User user);
    Task UpdateUserByIdAsync(User user);
    Task DeleteUserByIdAsync(User user);
}


using EntityLayer.Entities;
using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(Guid userId);
    Task<User> GetByPredicateAsync(Expression<Func<User, bool>> predicate);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(User user);
}


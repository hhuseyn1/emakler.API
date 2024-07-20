using DTO.User;

namespace BusinessLayer.Interfaces.UserServices;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto> GetUserByIdAsync(Guid userId);
    Task<UserDto> GetUserByContactNumberAsync(string contactNumber);
    Task<UserDto> GetUserByEmailAsync(string email);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    Task<UserDto> UpdateUserbyIdAsync(Guid userId, UpdateUserDto updateUserDto);
    Task<bool> DeleteUserbyIdAsync(Guid userId);
}

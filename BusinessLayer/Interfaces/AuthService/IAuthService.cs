using DTO.Auth;
using DTO.User;

namespace BusinessLayer.Interfaces.AuthService;

public interface IAuthService
{
    Task<string> LoginUserAsync(UserLoginRequest request);
    Task<UserDto> RegisterUserAsync(UserRegisterRequest request);
}

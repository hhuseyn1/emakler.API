using DTO.Auth;
using DTO.User;

namespace BusinessLayer.Interfaces.AuthService;

public interface IAuthService
{
    Task<AuthResponse> LoginUserAsync(UserLoginRequest request);
    Task<UserDto> RegisterUserAsync(UserRegisterRequest request);
}

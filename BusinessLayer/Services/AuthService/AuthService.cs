using BusinessLayer.Interfaces.AuthService;
using DTO.Auth;
using DTO.User;

namespace BusinessLayer.Services.AuthService;

public class AuthService : IAuthService
{
    public Task<AuthResponse> LoginUserAsync(UserLoginRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> RegisterUserAsync(UserRegisterRequest request)
    {
        throw new NotImplementedException();
    }
}

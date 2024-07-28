using AutoMapper;
using BusinessLayer.Interfaces.AuthService;
using DataAccessLayer.Interfaces;
using DTO.Auth;
using DTO.User;

namespace BusinessLayer.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public AuthService(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public Task<AuthResponse> LoginUserAsync(UserLoginRequest request)
    {
        throw new NotImplementedException();
    }
    

    public Task<UserDto> RegisterUserAsync(UserRegisterRequest request)
    {
        throw new NotImplementedException();
    }

}

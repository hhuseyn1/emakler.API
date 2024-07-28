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
    

        var token = _jwtService.GenerateSecurityToken(new UserDto
        {
            Email = userDto.Email,
            ContactNumber = userDto.ContactNumber
        });

        _logger.LogInformation($"Login successful for email: {request.Email}. Token generated.");
        return token;
    }

    public async Task<UserDto> RegisterUserAsync(UserRegisterRequest request)
    {
        var validationResult = await _registerValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning($"Registration validation failed for email {request.Email}: {string.Join(", ", validationResult.Errors)}");
            throw new ValidationException(validationResult.Errors);
        }

        if (await _userService.GetUserByEmailAsync(request.Email) != null)
        {
            _logger.LogWarning($"User already exists with email: {request.Email}");
            throw new InvalidOperationException("User already exists");
        }

        var passwordHashSalt = _jwtService.CreatePasswordHash(request.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserMail = request.Email,
            ContactNumber = request.PhoneNumber,
            PasswordHash = passwordHashSalt.PasswordHash,
            PasswordSalt = passwordHashSalt.PasswordSalt,
            OtpCreatedTime = DateTime.UtcNow,
            IsValidate = false
        };

        _logger.LogInformation($"Registering new user: {user}");
        await _userRepository.AddUserAsync(user);

        var userDto = new UserDto
        {
            Email = user.UserMail,
            ContactNumber = user.ContactNumber,
            OtpCreatedTime = user.OtpCreatedTime,
            IsVerified = user.IsValidate
        };

        _logger.LogInformation($"User registered successfully: {userDto}");
        return userDto;
    }

    public async Task<string> SendOtpAsync(string phoneNumber)
    {
        return await _otpService.GenerateOtpAsync(phoneNumber);
    }

}

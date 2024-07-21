using BusinessLayer.Interfaces.AuthService;
using BusinessLayer.Interfaces.OtpService;
using BusinessLayer.Interfaces.UserServices;
using BusinessLayer.Services.JwtService;
using DataAccessLayer.Interfaces;
using DTO.Auth;
using DTO.User;
using EntityLayer.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly JWTService _jwtService;
    private readonly ILogger<AuthService> _logger;
    private readonly IValidator<UserRegisterRequest> _registerValidator;
    private readonly IValidator<UserLoginRequest> _loginValidator;
    private readonly IOtpService _otpService;

    public AuthService(
        IUserRepository userRepository,
        JWTService jwtService,
        ILogger<AuthService> logger,
        IValidator<UserRegisterRequest> registerValidator,
        IValidator<UserLoginRequest> loginValidator,
        IUserService userService,
        IOtpService otpService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _logger = logger;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
        _userService = userService;
        _otpService = otpService;
    }

    public async Task<string> LoginUserAsync(UserLoginRequest request)
    {
        var validationResult = await _loginValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning($"Login validation failed for email {request.Email}: {string.Join(", ", validationResult.Errors)}");
            throw new ValidationException(validationResult.Errors);
        }

        _logger.LogInformation($"Attempting login for email: {request.Email}");
        var userDto = await _userService.GetUserByEmailAsync(request.Email);
        var user = await _userRepository.GetUserByIdAsync(userDto.Id);
        if (userDto == null || !_jwtService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            _logger.LogWarning($"Invalid login attempt for email: {request.Email}");
            throw new UnauthorizedAccessException("Invalid credentials");
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
        return await _otpService.SendOtpAsync(phoneNumber);
    }

    public async Task<bool> VerifyOtpAsync(string phoneNumber, string otpCode)
    {
        return await _otpService.VerifyOtpAsync(phoneNumber, otpCode);
    }
}

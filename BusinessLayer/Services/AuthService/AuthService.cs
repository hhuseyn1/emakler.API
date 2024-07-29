using AutoMapper;
using BusinessLayer.Configurations;
using BusinessLayer.Interfaces.AuthService;
using BusinessLayer.Interfaces.OtpService;
using BusinessLayer.Services.JwtService;
using DataAccessLayer.Interfaces;
using DTO.Auth;
using DTO.User;
using EntityLayer.Entities;
using Serilog;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IOtpService _otpService;
    private readonly JWTService _jwtService;
    private readonly JwtSettings _jwtSettings;

    public AuthService(IUserRepository userRepository, IMapper mapper, IOtpService otpService, JWTService jwtService, JwtSettings jwtSettings)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _otpService = otpService;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings;
    }

    public async Task<UserDto> RegisterUserAsync(UserRegisterRequest request)
    {
        var userExists = await _userRepository.GetUserByPredicateAsync(u => u.ContactNumber == request.ContactNumber);
        if (userExists is not null)
        {
            Log.Warning($"User registration attempt with existing email: {request.Email}");
            throw new ArgumentException("User already exists.");
        }

        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = _mapper.Map<User>(request);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        await _userRepository.AddUserAsync(user);

        Log.Information($"User registered: {request.Email}");

        return _mapper.Map<UserDto>(user);
    }

    public async Task<AuthResponse> LoginUserAsync(UserLoginRequest request)
    {
        var user = await _userRepository.GetUserByPredicateAsync(u => u.ContactNumber == request.ContactNumber);
        if (user == null || !VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            Log.Warning($"Login attempt failed for contact number: {request.ContactNumber}");
            throw new ArgumentException("Invalid username or password.");
        }

        var userDto = _mapper.Map<UserDto>(user);
        var accessToken = _jwtService.GenerateSecurityToken(userDto);
        var refreshToken = _jwtService.GenerateRefreshToken();

        Log.Information($"User logged in: {request.ContactNumber}");

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
    }

    public async Task<string> RefreshTokenAsync(string refreshToken)
    {
        var newAccessToken = _jwtService.GenerateAccessTokenFromRefreshToken(refreshToken, _jwtSettings.SecretKey);
        Log.Information("Generated new access token from refresh token.");
        return newAccessToken;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(storedHash);
    }
}

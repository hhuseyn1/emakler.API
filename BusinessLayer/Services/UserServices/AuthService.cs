using BusinessLayer.Interfaces.UserServices;
using DataAccessLayer.Interfaces;
using DTO.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLayer.Services.UserServices;

public class AuthService : BaseService, IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepository, IConfiguration configuration,ILogger<AuthService> logger)
        :base(logger)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<string> Login(UserLoginRequest request)
    {
        var user = await _userRepository.GetUserByUsernameAsync(request.Email);
        if (user == null || !VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, request.Email),
                new Claim(ClaimTypes.Email, request.Email),
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }

    public async Task<bool> VerifyOtpAsync(VerifyOtpRequest request)
    {
        var user = await _userRepository.GetUserByContactNumberAsync(request.ContactNumber);

        if (user == null || user.OtpCode != request.OtpCode || (DateTime.UtcNow - user.OtpCreatedTime).TotalMinutes > 10)
            return false;

        user.IsValidate = true;
        user.OtpCode = null;
        user.OtpCreatedTime = DateTime.MinValue;

        var updateUserDto = new UpdateUserDto
        {
            UserMail = user.UserMail,
            ContactNumber = user.ContactNumber,
            OtpCode = null,
            OtpCreatedTime = DateTime.MinValue,
            IsValidate = true
            //Password = user.UserPassword
        };

        await _userRepository.UpdateUserAsync(updateUserDto);
        return true;
    }
}

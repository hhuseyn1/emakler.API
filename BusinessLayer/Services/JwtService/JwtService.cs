using BusinessLayer.Configurations;
using DTO.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer.Services.JwtService;

public class JWTService
{
    private readonly JwtSettings _jwtSettings;

    public JWTService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateSecurityToken(UserDto user)
    {
        List<Claim> Claims = new()
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.MobilePhone, user.ContactNumber)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            claims: Claims,
            signingCredentials: creds,
            expires: DateTime.Now.AddDays(_jwtSettings.ExpiryDay)
            );

        var JWT = new JwtSecurityTokenHandler().WriteToken(token);
        return JWT;
    }
    public (byte[] PasswordHash, byte[] PasswordSalt) CreatePasswordHash(string password)
    {
        using (var hmac = new HMACSHA512())
        {
            var passwordSalt = hmac.Key;
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return (passwordHash, passwordSalt);
        }
    }

    public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using (var hmac = new HMACSHA512(storedSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }
    }
}
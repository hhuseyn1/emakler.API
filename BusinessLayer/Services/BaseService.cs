using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer.Services;

public abstract class BaseService
{
    protected readonly ILogger<BaseService> _logger;

    protected BaseService(ILogger<BaseService> logger)
    {
        _logger = logger;
    }

    protected void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    protected bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(storedHash);
    }
}

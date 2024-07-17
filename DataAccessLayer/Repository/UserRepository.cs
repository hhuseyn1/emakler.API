using DataAccessLayer.Concrete;
using DataAccessLayer.Interfaces;
using DTO.User;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.Repository;

public class UserRepository : IUserRepository
{
    private readonly Context _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(Context context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddUserAsync(AddUserDto addUserDto)
    {
        try
        {
            CreatePasswordHash(addUserDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserMail = addUserDto.UserMail,
                ContactNumber = addUserDto.ContactNumber,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                OtpCode = null,
                OtpCreatedTime = DateTime.MinValue,
                IsValidate = false
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User added successfully with email: {Email}", addUserDto.UserMail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding the user");
            throw;
        }
    }

    public async Task<User> GetUserByUsernameAsync(string userMail)
    {
        try
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserMail == userMail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the user by username: {UserMail}", userMail);
            throw;
        }
    }

    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        try
        {
            return await _context.Users.FindAsync(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the user by ID: {UserId}", userId);
            throw;
        }
    }

    public async Task<User> GetUserByContactNumberAsync(string contactNumber)
    {
        try
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.ContactNumber == contactNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the user by contact number: {ContactNumber}", contactNumber);
            throw;
        }
    }

    public async Task UpdateUserAsync(UpdateUserDto updateUserDto)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserMail == updateUserDto.UserMail);

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            if (!string.IsNullOrEmpty(updateUserDto.ContactNumber))
            {
                user.ContactNumber = updateUserDto.ContactNumber;
            }

            if (!string.IsNullOrEmpty(updateUserDto.OtpCode))
            {
                user.OtpCode = updateUserDto.OtpCode;
            }

            if (updateUserDto.OtpCreatedTime.HasValue)
            {
                user.OtpCreatedTime = updateUserDto.OtpCreatedTime.Value;
            }

            if (updateUserDto.IsValidate.HasValue)
            {
                user.IsValidate = updateUserDto.IsValidate.Value;
            }

            if (!string.IsNullOrEmpty(updateUserDto.Password))
            {
                CreatePasswordHash(updateUserDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User updated successfully with email: {UserMail}", updateUserDto.UserMail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the user with email: {UserMail}", updateUserDto.UserMail);
            throw;
        }
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User deleted successfully with ID: {UserId}", userId);
            }
            else
            {
                _logger.LogWarning("User not found with ID: {UserId}", userId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the user with ID: {UserId}", userId);
            throw;
        }
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}

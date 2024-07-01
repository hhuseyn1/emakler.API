using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.KafkaServices;
using BusinessLayer.Interfaces.UserServices;
using DataAccessLayer.Interfaces;
using DTO.User;
using EntityLayer.Entities;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer.Services;

public class UserService : IUserService
{
    private readonly IOtpService _otpService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;
    private readonly IProducerKafkaService _produceKafkaService;

    public UserService(
        IOtpService otpService,
        IUserRepository userRepository,
        ILogger<UserService> logger,
        IProducerKafkaService produceKafkaService)
    {
        _otpService = otpService;
        _userRepository = userRepository;
        _logger = logger;
        _produceKafkaService = produceKafkaService;
    }

    public async Task RegisterUser(UserRegistration userRegistration)
    {
        var existingUser = await _userRepository.GetUserByUsernameAsync(userRegistration.Email);
        if (existingUser != null)
            throw new ArgumentException("User already exists with this email.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserMail = userRegistration.Email,
            ContactNumber = userRegistration.ContactNumber,
            UserPassword = userRegistration.Password,
            IsValidate = false
        };

        CreatePasswordHash(userRegistration.Password, out byte[] passwordHash, out byte[] passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        await _userRepository.AddUserAsync(user);

        await _otpService.SendOtpAsync(userRegistration.ContactNumber);

        await _produceKafkaService.ProduceAsync("UserRegistered", userRegistration.ContactNumber);

        _logger.LogInformation($"User registered successfully with email: {userRegistration.Email}");
    }

    public async Task UpdateUser(Guid userId, UserRegistration userRegistration)
    {
        // Check if the user exists
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
            throw new ArgumentException("User does not exist.");

        // Update user details
        user.UserMail = userRegistration.Email;
        user.ContactNumber = userRegistration.ContactNumber;
        user.UserPassword = userRegistration.Password;

        // Hash the new password
        CreatePasswordHash(userRegistration.Password, out byte[] passwordHash, out byte[] passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        await _userRepository.UpdateUserAsync(user);

        await _produceKafkaService.ProduceAsync("UserUpdated", userId.ToString());

        _logger.LogInformation($"User updated successfully with ID: {userId}");
    }

    public async Task DeleteUser(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
            throw new ArgumentException("User does not exist.");

        await _userRepository.DeleteUserAsync(userId);

        await _produceKafkaService.ProduceAsync("UserDeleted", userId.ToString());

        _logger.LogInformation($"User deleted successfully with ID: {userId}");
    }

    public async Task<User> GetUserByMailAsync(string userMail)
    {
        return await _userRepository.GetUserByUsernameAsync(userMail);
    }

    public async Task<User> GetUserByContactNumberAsync(string contactNumber)
    {
        return await _userRepository.GetUserByContactNumberAsync(contactNumber);
    }

    public async Task<bool> ValidateUser(string userMail, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(userMail);
        if (user == null)
            return false;

        return VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using (var hmac = new HMACSHA512(storedSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }
    }
}

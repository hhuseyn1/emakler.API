using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.KafkaServices;
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
    private readonly IProducerKafkaService _producerKafkaService;

    public UserService(
        IOtpService otpService,
        IUserRepository userRepository,
        ILogger<UserService> logger,
        IProducerKafkaService producerKafkaService)
    {
        _otpService = otpService;
        _userRepository = userRepository;
        _logger = logger;
        _producerKafkaService = producerKafkaService;
    }

    public async Task RegisterUser(UserRegistration userRegistration)
    {
        // Check if the user already exists
        var existingUser = await _userRepository.GetUserByUsernameAsync(userRegistration.Email);
        if (existingUser != null)
            throw new ArgumentException("User already exists with this email.");

        // Create a new user
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserMail = userRegistration.Email,
            ContactNumber = userRegistration.ContactNumber,
            UserPassword = userRegistration.Password,
            IsValidate = false
        };

        // Hash the password
        CreatePasswordHash(userRegistration.Password, out byte[] passwordHash, out byte[] passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        // Add the user to the repository
        await _userRepository.AddUserAsync(user);

        // Send OTP to the user's contact number
        await _otpService.SendOtpAsync(userRegistration.ContactNumber);

        // Produce an event to Kafka
        await _producerKafkaService.Produce("UserRegistered", userRegistration.ContactNumber);

        // Log the registration
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

        // Update the user in the repository
        await _userRepository.UpdateUserAsync(user);

        // Produce an event to Kafka
        await _producerKafkaService.Produce("UserUpdated", userId.ToString());

        // Log the update
        _logger.LogInformation($"User updated successfully with ID: {userId}");
    }

    public async Task DeleteUser(Guid userId)
    {
        // Check if the user exists
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
            throw new ArgumentException("User does not exist.");

        // Delete the user
        await _userRepository.DeleteUserAsync(userId);

        // Produce an event to Kafka
        await _producerKafkaService.Produce("UserDeleted", userId.ToString());

        // Log the deletion
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
        // Get the user by mail
        var user = await _userRepository.GetUserByUsernameAsync(userMail);
        if (user == null)
            return false;

        // Verify the password
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

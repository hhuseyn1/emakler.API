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
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;
    private readonly IOtpService _otpService;
    private readonly IProducerKafkaService _produceKafkaService;

    public UserService(
        IUserRepository userRepository,
        ILogger<UserService> logger,
        IOtpService otpService,
        IProducerKafkaService produceKafkaService)
    {
        _userRepository = userRepository;
        _logger = logger;
        _otpService = otpService;
        _produceKafkaService = produceKafkaService;
    }

    public async Task RegisterUser(AddUserDto addUserDto)
    {
        var existingUser = await _userRepository.GetUserByUsernameAsync(addUserDto.UserMail);
        if (existingUser != null) throw new ArgumentException("User already exists with this email.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserMail = addUserDto.UserMail,
            ContactNumber = addUserDto.ContactNumber,
            IsValidate = false
        };

        CreatePasswordHash(addUserDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        await _userRepository.AddUserAsync(new AddUserDto
        {
            UserMail = user.UserMail,
            ContactNumber = user.ContactNumber,
            Password = addUserDto.Password
        });

        await _otpService.SendOtpAsync(new SendOtpRequest { ContactNumber = addUserDto.ContactNumber });

        await _produceKafkaService.ProduceAsync("UserRegistered", user.Id.ToString());

        _logger.LogInformation($"User registered successfully with email: {addUserDto.UserMail}");
    }

    public async Task UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) throw new ArgumentException("User does not exist.");

        if (updateUserDto.ContactNumber != null)
            user.ContactNumber = updateUserDto.ContactNumber;

        if (updateUserDto.OtpCode != null)
            user.OtpCode = updateUserDto.OtpCode;

        if (updateUserDto.OtpCreatedTime.HasValue)
            user.OtpCreatedTime = updateUserDto.OtpCreatedTime.Value;

        if (updateUserDto.IsValidate.HasValue)
            user.IsValidate = updateUserDto.IsValidate.Value;

        if (updateUserDto.Password != null)
        {
            CreatePasswordHash(updateUserDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
        }

        await _userRepository.UpdateUserAsync(new UpdateUserDto
        {
            UserMail = user.UserMail,
            ContactNumber = user.ContactNumber,
            OtpCode = user.OtpCode,
            OtpCreatedTime = user.OtpCreatedTime,
            IsValidate = user.IsValidate,
            Password = updateUserDto.Password
        });

        await _produceKafkaService.ProduceAsync("UserUpdated", userId.ToString());

        _logger.LogInformation($"User updated successfully with ID: {userId}");
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) throw new ArgumentException("User does not exist.");

        await _userRepository.DeleteUserAsync(userId);

        await _produceKafkaService.ProduceAsync("UserDeleted", userId.ToString());

        _logger.LogInformation($"User deleted successfully with ID: {userId}");
    }

    public async Task<UserDto> GetUserByIdAsync(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) throw new ArgumentException("User not found.");
        return new UserDto
        {
            Id = user.Id,
            UserMail = user.UserMail,
            ContactNumber = user.ContactNumber,
            IsEmailConfirmed = user.IsValidate,
            CreatedDate = user.OtpCreatedTime
        };
    }

    public async Task<UserDto> GetUserByMailAsync(string userMail)
    {
        var user = await _userRepository.GetUserByUsernameAsync(userMail);
        if (user == null) throw new ArgumentException("User not found.");
        return new UserDto
        {
            Id = user.Id,
            UserMail = user.UserMail,
            ContactNumber = user.ContactNumber,
            IsEmailConfirmed = user.IsValidate,
            CreatedDate = user.OtpCreatedTime
        };
    }

    public async Task<UserDto> GetUserByContactNumberAsync(string contactNumber)
    {
        var user = await _userRepository.GetUserByContactNumberAsync(contactNumber);
        if (user == null) throw new ArgumentException("User not found.");
        return new UserDto
        {
            Id = user.Id,
            UserMail = user.UserMail,
            ContactNumber = user.ContactNumber,
            IsEmailConfirmed = user.IsValidate,
            CreatedDate = user.OtpCreatedTime
        };
    }

    public async Task<bool> ValidateUserAsync(string userMail, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(userMail);
        if (user == null) return false;

        return VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
    }

    public async Task<bool> SendOtpAsync(SendOtpRequest request)
    {
        var otp = new Random().Next(100000, 999999).ToString();
        var user = await _userRepository.GetUserByContactNumberAsync(request.ContactNumber);
        if (user != null)
        {
            user.OtpCode = otp;
            user.OtpCreatedTime = DateTime.UtcNow;
            await _userRepository.UpdateUserAsync(new UpdateUserDto
            {
                UserMail = user.UserMail,
                ContactNumber = user.ContactNumber,
                OtpCode = otp,
                OtpCreatedTime = user.OtpCreatedTime,
                IsValidate = user.IsValidate
            });

            await _otpService.SendOtpAsync(request);

            return true;
        }

        return false;
    }

    public async Task<bool> VerifyOtpAsync(VerifyOtpRequest request)
    {
        var user = await _userRepository.GetUserByContactNumberAsync(request.ContactNumber);
        if (user == null || user.OtpCode != request.OtpCode || (DateTime.UtcNow - user.OtpCreatedTime).TotalMinutes > 10)
            return false;

        user.IsValidate = true;
        user.OtpCode = null;
        user.OtpCreatedTime = DateTime.MinValue;

        await _userRepository.UpdateUserAsync(new UpdateUserDto
        {
            UserMail = user.UserMail,
            ContactNumber = user.ContactNumber,
            OtpCode = user.OtpCode,
            OtpCreatedTime = user.OtpCreatedTime,
            IsValidate = user.IsValidate
        });

        return true;
    }

    public async Task ConfirmEmailAsync(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) throw new ArgumentException("User not found.");

        user.IsValidate = true;
        await _userRepository.UpdateUserAsync(new UpdateUserDto
        {
            UserMail = user.UserMail,
            ContactNumber = user.ContactNumber,
            OtpCode = user.OtpCode,
            OtpCreatedTime = user.OtpCreatedTime,
            IsValidate = user.IsValidate
        });
    }

    public async Task ChangePasswordAsync(ChangePasswordRequest request)
    {
        var user = await _userRepository.GetUserByIdAsync(request.UserId);
        if (user == null) throw new ArgumentException("User not found.");

        if (!VerifyPasswordHash(request.CurrentPassword, user.PasswordHash, user.PasswordSalt))
            throw new ArgumentException("Current password is incorrect.");

        CreatePasswordHash(request.NewPassword, out byte[] newPasswordHash, out byte[] newPasswordSalt);

        user.PasswordHash = newPasswordHash;
        user.PasswordSalt = newPasswordSalt;

        await _userRepository.UpdateUserAsync(new UpdateUserDto
        {
            UserMail = user.UserMail,
            ContactNumber = user.ContactNumber,
            OtpCode = user.OtpCode,
            OtpCreatedTime = user.OtpCreatedTime,
            IsValidate = user.IsValidate,
            Password = request.NewPassword
        });
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

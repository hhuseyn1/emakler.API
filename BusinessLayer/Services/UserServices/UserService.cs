using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.KafkaServices;
using BusinessLayer.Interfaces.UserServices;
using DataAccessLayer.Interfaces;
using DTO.User;
using EntityLayer.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services;

public class UserService : BaseService, IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IOtpService _otpService;
    private readonly IProducerKafkaService _produceKafkaService;

    public UserService(
        IUserRepository userRepository,
        ILogger<UserService> logger,
        IOtpService otpService,
        IProducerKafkaService produceKafkaService)
        : base(logger)
    {
        _userRepository = userRepository;
        _otpService = otpService;
        _produceKafkaService = produceKafkaService;
    }

    public async Task RegisterUser(UserRegistration userRegistration)
    {
        var existingUserDto = await _userRepository.GetUserByUsernameAsync(userRegistration.Email);
        if (existingUserDto != null)
        {
            throw new ArgumentException("User already exists with this email.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserMail = userRegistration.Email,
            ContactNumber = userRegistration.ContactNumber,
            IsValidate = false
        };

        CreatePasswordHash(userRegistration.Password, out byte[] passwordHash, out byte[] passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        await _userRepository.AddUserAsync(user);

        await _produceKafkaService.ProduceAsync(user.Id.ToString(), "User Registered");

        _logger.LogInformation($"User registered with email: {userRegistration.Email}");
    }


    public async Task UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) throw new ArgumentException("User not found.");

        user.UserMail = updateUserDto.UserMail ?? user.UserMail;
        user.ContactNumber = updateUserDto.ContactNumber ?? user.ContactNumber;

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
            OtpCode = updateUserDto.OtpCode ?? user.OtpCode,
            OtpCreatedTime = updateUserDto.OtpCreatedTime ?? user.OtpCreatedTime,
            IsValidate = updateUserDto.IsValidate ?? user.IsValidate,
            Password = updateUserDto.Password ?? user.UserPassword
        });

        _logger.LogInformation($"User updated with ID: {userId}");
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        await _userRepository.DeleteUserAsync(userId);
        _logger.LogInformation($"User deleted with ID: {userId}");
    }

    public async Task<UserDto> GetUserByIdAsync(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        return user == null ? null : new UserDto
        {
            Id = user.Id,
            UserMail = user.UserMail,
            ContactNumber = user.ContactNumber,
            IsEmailConfirmed = user.IsValidate
        };
    }

    public async Task<UserDto> GetUserByMailAsync(string userMail)
    {
        var user = await _userRepository.GetUserByUsernameAsync(userMail);
        return user == null ? null : new UserDto
        {
            Id = user.Id,
            UserMail = user.UserMail,
            ContactNumber = user.ContactNumber,
            IsEmailConfirmed = user.IsValidate
        };
    }

    public async Task<UserDto> GetUserByContactNumberAsync(string contactNumber)
    {
        var user = await _userRepository.GetUserByContactNumberAsync(contactNumber);
        return user == null ? null : new UserDto
        {
            Id = user.Id,
            UserMail = user.UserMail,
            ContactNumber = user.ContactNumber,
            IsEmailConfirmed = user.IsValidate
        };
    }

    public async Task<bool> ValidateUserAsync(string userMail, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(userMail);
        return user != null && VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
    }

    public async Task<bool> SendOtpAsync(SendOtpRequest request)
    {
        return await _otpService.SendOtpAsync(request);
    }

    public async Task<bool> VerifyOtpAsync(VerifyOtpRequest request)
    {
        return await _otpService.VerifyOtpAsync(request);
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
            IsValidate = true,
            Password = user.UserPassword
        });

        _logger.LogInformation($"User email confirmed for ID: {userId}");
    }

    public async Task ChangePasswordAsync(ChangePasswordRequest request)
    {
        var user = await _userRepository.GetUserByIdAsync(request.UserId);
        if (user == null) throw new ArgumentException("User not found.");

        CreatePasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        await _userRepository.UpdateUserAsync(new UpdateUserDto
        {
            UserMail = user.UserMail,
            ContactNumber = user.ContactNumber,
            OtpCode = user.OtpCode,
            OtpCreatedTime = user.OtpCreatedTime,
            IsValidate = user.IsValidate,
            Password = request.NewPassword
        });

        _logger.LogInformation($"Password changed for user: {user.ContactNumber}");
    }
}

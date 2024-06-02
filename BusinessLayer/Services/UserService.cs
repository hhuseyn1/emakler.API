using BusinessLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DTO.User;
using EntityLayer.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BusinessLayer.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private static ConcurrentDictionary<string, string> _otpStore = new ConcurrentDictionary<string, string>();

    public UserService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;

        var accountSid = _configuration["Twilio:AccountSid"];
        var authToken = _configuration["Twilio:AuthToken"];
        TwilioClient.Init(accountSid, authToken);
    }

    public async Task RegisterUser(UserRegistration userRegistration)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserMail = userRegistration.UserMail,
            ContactNumber = userRegistration.ContactNumber,
            UserPassword = userRegistration.UserPassword, 
            IsValidate = false
        };

        CreatePasswordHash(userRegistration.UserPassword, out byte[] passwordHash, out byte[] passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        await _userRepository.AddUserAsync(user);
    }

    public async Task<bool> ValidateUser(string userMail, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(userMail);
        if (user == null)
            return false;

        return VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
    }

    public async Task SendOtp(string contactNumber)
    {
        var otp = new Random().Next(100000, 999999).ToString();
        var user = await _userRepository.GetUserByContactNumberAsync(contactNumber);
        if (user != null)
        {
            user.OtpCode = otp;
            user.OtpCreatedTime = DateTime.UtcNow;
            await _userRepository.UpdateUserAsync(user);
        }

        var fromPhoneNumber = _configuration["Twilio:PhoneNumber"];

        var message = await MessageResource.CreateAsync(
            body: $"Your OTP code is {otp}",
            from: new PhoneNumber(fromPhoneNumber),
            to: new PhoneNumber(contactNumber)
        );
    }

    public async Task<bool> VerifyOtp(string contactNumber, string otpCode)
    {
        var user = await _userRepository.GetUserByContactNumberAsync(contactNumber);
        if (user == null || user.OtpCode != otpCode || (DateTime.UtcNow - user.OtpCreatedTime).TotalMinutes > 10)
            return false;

        user.IsValidate = true;
        user.OtpCode = null;
        user.OtpCreatedTime = DateTime.MinValue;
        await _userRepository.UpdateUserAsync(user);
        return true;
    }

    public async Task UpdateUser(Guid userId, UserRegistration userRegistration)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user != null)
        {
            user.UserMail = userRegistration.UserMail;
            user.ContactNumber = userRegistration.ContactNumber;
            user.UserPassword = userRegistration.UserPassword; 

            CreatePasswordHash(userRegistration.UserPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _userRepository.UpdateUserAsync(user);
        }
    }

    public async Task DeleteUser(Guid userId)
    {
        await _userRepository.DeleteUserAsync(userId);
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }


    private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using (var hmac = new HMACSHA512(storedSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i]) return false;
            }
        }
        return true;
    }

    public async Task<bool> RequestPasswordReset(string phoneNumber)
    {
        var user = await _userRepository.GetUserByContactNumberAsync(phoneNumber);
        if (user != null)
        {
            await SendOtp(phoneNumber);
            return true;
        }
        return false;
    }

    public async Task<bool> ConfirmPasswordReset(ConfirmResetPasswordRequest request)
    {
        var user = await _userRepository.GetUserByContactNumberAsync(request.PhoneNumber);
        if (user != null && user.OtpCode == request.OtpCode && (DateTime.UtcNow - user.OtpCreatedTime).TotalMinutes <= 5)
        {
            using var hmac = new HMACSHA512();
            user.PasswordSalt = hmac.Key;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.NewPassword));
            user.OtpCode = null;
            await _userRepository.UpdateUserAsync(user);
            return true;
        }
        return false;
    }
}

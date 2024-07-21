using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;
using DataAccessLayer.Interfaces;
using EntityLayer.Entities;
using BusinessLayer.Interfaces.OtpService;

public class OtpService : IOtpService
{
    private readonly IUserRepository _userRepository;
    private readonly FirebaseMessaging _messaging;
    private readonly ILogger<OtpService> _logger;

    public OtpService(IUserRepository userRepository, ILogger<OtpService> logger)
    {
        _userRepository = userRepository;
        _messaging = FirebaseMessaging.DefaultInstance;
        _logger = logger;
    }

    public async Task<string> SendOtpAsync(string phoneNumber)
    {
        var otpCode = new Random().Next(100000, 999999).ToString();
        var user = await _userRepository.GetByPredicateAsync(u => u.ContactNumber == phoneNumber);

        if (user == null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                ContactNumber = phoneNumber,
                OtpCode = otpCode,
                OtpExpiryTime = DateTime.UtcNow.AddMinutes(5)
            };
            await _userRepository.AddUserAsync(user);
        }
        else
        {
            user.OtpCode = otpCode;
            user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(5);
            await _userRepository.UpdateUserAsync(user);
        }

        var message = new Message()
        {
            Data = new Dictionary<string, string>()
            {
                { "otp", otpCode }
            },
            Token = phoneNumber,
            Notification = new Notification
            {
                Title = "Your OTP Code",
                Body = $"Your OTP code is {otpCode}"
            }
        };

        await _messaging.SendAsync(message);
        _logger.LogInformation($"OTP generated and sent to phone number: {phoneNumber}");

        return otpCode;
    }

    public async Task<bool> VerifyOtpAsync(string phoneNumber, string otpCode)
    {
        var user = await _userRepository.GetByPredicateAsync(u=>u.ContactNumber == phoneNumber);

        if (user == null || user.OtpCode != otpCode || user.OtpExpiryTime < DateTime.UtcNow)
        {
            return false;
        }

        user.OtpCode = null;
        user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(-1);
        await _userRepository.UpdateUserAsync(user);

        return true;
    }
}

using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.UserServices;
using Confluent.Kafka;
using DataAccessLayer.Interfaces;
using DTO.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BusinessLayer.Services;

public class OtpService : BaseService, IOtpService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ConcurrentDictionary<string, (string OtpCode, DateTime CreatedAt)> _otpStore = new();

    public OtpService(IUserRepository userRepository, ILogger<OtpService> logger, IConfiguration configuration)
        : base(logger)
    {
        _userRepository = userRepository;
        _configuration = configuration;
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
                ContactNumber = user.ContactNumber,
                OtpCode = otp,
                OtpCreatedTime = user.OtpCreatedTime,
                IsValidate = user.IsValidate
            });

            var fromPhoneNumber = _configuration["Twilio:PhoneNumber"];
            var message = await MessageResource.CreateAsync(
                body: $"Your OTP code is {otp}",
                from: new PhoneNumber(fromPhoneNumber),
                to: new PhoneNumber(request.ContactNumber)
            );

            _otpStore[request.ContactNumber] = (otp, DateTime.UtcNow);
            _logger.LogInformation($"OTP sent to {request.ContactNumber}: {otp}");
            return true;
        }

        return false;
    }

    public async Task<bool> VerifyOtpAsync(VerifyOtpRequest request)
    {
        if (_otpStore.TryGetValue(request.ContactNumber, out var otpInfo))
        {
            if (otpInfo.OtpCode == request.OtpCode && (DateTime.UtcNow - otpInfo.CreatedAt).TotalMinutes <= 10)
            {
                _otpStore.TryRemove(request.ContactNumber, out _);

                var user = await _userRepository.GetUserByContactNumberAsync(request.ContactNumber);
                if (user != null)
                {
                    user.IsValidate = true;
                    user.OtpCode = null;
                    user.OtpCreatedTime = DateTime.MinValue;
                    await _userRepository.UpdateUserAsync(new UpdateUserDto
                    {
                        ContactNumber = user.ContactNumber,
                        OtpCode = null,
                        OtpCreatedTime = DateTime.MinValue,
                        IsValidate = true
                    });
                    _logger.LogInformation($"OTP verified for {request.ContactNumber}");
                    return true;
                }
            }
        }

        _logger.LogWarning($"Failed OTP verification for {request.ContactNumber}");
        return false;
    }
}

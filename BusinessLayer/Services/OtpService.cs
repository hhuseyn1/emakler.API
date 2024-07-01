using BusinessLayer.Interfaces;
using DataAccessLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BusinessLayer.Services;

public class OtpService : IOtpService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private static ConcurrentDictionary<string, (string OtpCode, DateTime CreatedAt)> _otpStore = new ConcurrentDictionary<string, (string, DateTime)>();

    public OtpService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;

        var accountSid = _configuration["Twilio:AccountSid"];
        var authToken = _configuration["Twilio:AuthToken"];
        TwilioClient.Init(accountSid, authToken);
    }

    public async Task<bool> SendOtpAsync(string contactNumber)
    {
        var otp = new Random().Next(100000, 999999).ToString();
        var user = await _userRepository.GetUserByContactNumberAsync(contactNumber);
        if (user != null)
        {
            user.OtpCode = otp;
            user.OtpCreatedTime = DateTime.UtcNow;
            await _userRepository.UpdateUserAsync(user);

            var fromPhoneNumber = _configuration["Twilio:PhoneNumber"];

            var message = await MessageResource.CreateAsync(
                body: $"Your OTP code is {otp}",
                from: new PhoneNumber(fromPhoneNumber),
                to: new PhoneNumber(contactNumber)
            );

            _otpStore[contactNumber] = (otp, DateTime.UtcNow);
            return true;
        }

        return false;
    }

    public async Task<bool> VerifyOtpAsync(string contactNumber, string otpCode)
    {
        if (_otpStore.TryGetValue(contactNumber, out var otpInfo))
        {
            if (otpInfo.OtpCode == otpCode && (DateTime.UtcNow - otpInfo.CreatedAt).TotalMinutes <= 10)
            {
                _otpStore.TryRemove(contactNumber, out _);

                var user = await _userRepository.GetUserByContactNumberAsync(contactNumber);
                if (user != null)
                {
                    user.IsValidate = true;
                    user.OtpCode = null;
                    user.OtpCreatedTime = DateTime.MinValue;
                    await _userRepository.UpdateUserAsync(user);
                    return true;
                }
            }
        }

        return false;
    }
}

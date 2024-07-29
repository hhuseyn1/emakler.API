using BusinessLayer.Interfaces.OtpService;
using Twilio.Rest.Verify.V2.Service;
using Twilio;
using Microsoft.Extensions.Options;
using BusinessLayer.Configurations;
using Serilog;

namespace BusinessLayer.Services.OtpService;

public class OtpService : IOtpService
{
    private readonly TwilioSettings _twilioSettings;

    public OtpService(IOptions<TwilioSettings> twilioSettings)
    {
        _twilioSettings = twilioSettings?.Value ?? throw new ArgumentNullException(nameof(twilioSettings));
        TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);
    }

    public async Task SendOtpAsync(string phoneNumber)
    {
        try
        {
            var verification = await VerificationResource.CreateAsync(
                to: phoneNumber,
                channel: "sms",
                pathServiceSid: _twilioSettings.TwilioServiceSid
            );

            Log.Information($"OTP sent to {phoneNumber} via SMS.");
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, $"Error sending OTP to {phoneNumber}.");
            throw;
        }
    }

    public async Task<bool> VerifyOtpAsync(string phoneNumber, string otp)
    {
        try
        {
            var verificationCheck = await VerificationCheckResource.CreateAsync(
                to: phoneNumber,
                code: otp,
                pathServiceSid: _twilioSettings.TwilioServiceSid
            );

            var isApproved = verificationCheck.Status == "approved";
            Log.Information($"OTP verification for {phoneNumber} {(isApproved ? "approved" : "failed")}.");

            return isApproved;
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, $"Error verifying OTP for {phoneNumber}.");
            throw;
        }
    }
}

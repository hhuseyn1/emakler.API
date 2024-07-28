using BusinessLayer.Interfaces.OtpService;
using Twilio.Rest.Verify.V2.Service;
using Twilio;
using Microsoft.Extensions.Options;
using BusinessLayer.Configurations;

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
        var verification = await VerificationResource.CreateAsync(
            to: phoneNumber,
            channel: "sms",
            pathServiceSid: _twilioSettings.TwilioServiceSid
        );
    }

    public async Task<bool> VerifyOtpAsync(string phoneNumber, string otp)
    {
        var verificationCheck = await VerificationCheckResource.CreateAsync(
            to: phoneNumber,
            code: otp,
            pathServiceSid: _twilioSettings.TwilioServiceSid
        );

        return verificationCheck.Status == "approved";
    }
}

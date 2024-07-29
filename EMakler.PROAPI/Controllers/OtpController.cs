using BusinessLayer.Interfaces.OtpService;
using DTO.Otp;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EMakler.PROAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OtpController : ControllerBase
{
    private readonly IOtpService _otpService;

    public OtpController(IOtpService otpService)
    {
        _otpService = otpService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendOtp([FromBody] string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
        {
            Log.Information("Attempt to send OTP with an empty phone number.");
            return BadRequest("Phone number cannot be empty.");
        }

        await _otpService.SendOtpAsync(phoneNumber);

        return Ok(new { OtpSent = true });
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyOtp([FromBody] OtpVerificationRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.ContactNumber) || string.IsNullOrEmpty(request.OtpCode))
        {
            Log.Information("Invalid OTP verification request.");
            return BadRequest("Invalid OTP verification request.");
        }

        var isValid = await _otpService.VerifyOtpAsync(request.ContactNumber, request.OtpCode);

        if (!isValid)
        {
            return BadRequest("Invalid OTP.");
        }

        return Ok("OTP verified successfully.");
    }
}

using BusinessLayer.Interfaces.AuthService;
using BusinessLayer.Interfaces.OtpService;
using DTO.Otp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        var otp = await _otpService.SendOtpAsync(phoneNumber);
        return Ok(new { OtpSent = true });
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyOtp([FromBody] OtpVerificationRequest request)
    {
        var isValid = await _otpService.VerifyOtpAsync(request.ContactNumber, request.OtpCode);

        if (!isValid)
        {
            return BadRequest("Invalid OTP");
        }

        return Ok("OTP verified successfully");
    }
}

using BusinessLayer.Interfaces;
using DTO.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EMakler.PROAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public UserController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegistration userRegistration)
    {
        await _userService.RegisterUser(userRegistration);

        try
        {
            await _userService.SendOtp(userRegistration.ContactNumber);
            return Ok(new { Message = "User registered successfully. OTP sent to phone number." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = $"Failed to send OTP: {ex.Message}" });
        }
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(UserVerificationRequest request)
    {
        if (await _userService.VerifyOtp(request.ContactNumber, request.OtpCode))
        {
            return Ok(new { Message = "OTP verified successfully." });
        }
        return BadRequest(new { Message = "Invalid OTP." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRequest request)
    {
        if (await _userService.ValidateUser(request.UserMail, request.UserPassword))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Identity:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, request.UserMail)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new UserLoginResponse { Token = tokenString });
        }

        return Unauthorized(new { Message = "Invalid username or password." });
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(Guid userId, UserRegistration userRegistration)
    {
        await _userService.UpdateUser(userId, userRegistration);
        return Ok(new { Message = "User updated successfully." });
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await _userService.DeleteUser(userId);
        return Ok(new { Message = "User deleted successfully." });
    }

    [HttpPost("request-password-reset")]
    public async Task<IActionResult> RequestPasswordReset(ResetPasswordRequest request)
    {
        if (await _userService.RequestPasswordReset(request.ContactNumber))
        {
            return Ok(new { Message = "Password reset OTP sent to phone number." });
        }
        return NotFound(new { Message = "User not found." });
    }

    [HttpPost("confirm-password-reset")]
    public async Task<IActionResult> ConfirmPasswordReset(ConfirmResetPasswordRequest request)
    {
        if (await _userService.ConfirmPasswordReset(request))
        {
            return Ok(new { Message = "Password reset successfully." });
        }
        return BadRequest(new { Message = "Invalid OTP or OTP expired." });
    }
}

using BusinessLayer.Interfaces;
using DTO.User;
using EMakler.PROAPI.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EMakler.PROAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly JwtSettings _jwtSettings;
    private readonly IProducerKafkaService _producerKafkaService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IUserService userService,
        IOptions<JwtSettings> jwtSettings,
        IProducerKafkaService kafkaProducerService,
        ILogger<AuthController> logger)
    {
        _userService = userService;
        _jwtSettings = jwtSettings.Value;
        _producerKafkaService = kafkaProducerService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegistration userRegistration)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _userService.RegisterUser(userRegistration);
            await _userService.SendOtp(userRegistration.ContactNumber);
            await _producerKafkaService.Produce("UserRegistered", userRegistration.ContactNumber);
            return Ok(new { Message = "User registered successfully. OTP sent to phone number." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to register user and send OTP");
            return StatusCode(500, new { Message = $"Failed to send OTP: {ex.Message}" });
        }
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(UserVerificationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (await _userService.VerifyOtp(request.ContactNumber, request.OtpCode))
            {
                await _producerKafkaService.Produce("OtpVerified", request.ContactNumber);
                return Ok(new { Message = "OTP verified successfully." });
            }
            return BadRequest(new { Message = "Invalid OTP." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to verify OTP");
            return StatusCode(500, new { Message = $"Failed to verify OTP: {ex.Message}" });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (await _userService.ValidateUser(request.UserMail, request.UserPassword))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
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

                await _producerKafkaService.Produce("UserLoggedIn", request.UserMail);

                return Ok(new UserLoginResponse { Token = tokenString });
            }
            return Unauthorized(new { Message = "Invalid username or password." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed");
            return StatusCode(500, new { Message = $"Login failed: {ex.Message}" });
        }
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(Guid userId, UserRegistration userRegistration)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _userService.UpdateUser(userId, userRegistration);
            await _producerKafkaService.Produce("UserUpdated", userId.ToString());
            return Ok(new { Message = "User updated successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update user");
            return StatusCode(500, new { Message = $"Failed to update user: {ex.Message}" });
        }
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        try
        {
            await _userService.DeleteUser(userId);
            await _producerKafkaService.Produce("UserDeleted", userId.ToString());
            return Ok(new { Message = "User deleted successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete user");
            return StatusCode(500, new { Message = $"Failed to delete user: {ex.Message}" });
        }
    }

    [HttpPost("request-password-reset")]
    public async Task<IActionResult> RequestPasswordReset(ResetPasswordRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (await _userService.RequestPasswordReset(request.ContactNumber))
            {
                await _producerKafkaService.Produce("PasswordResetRequested", request.ContactNumber);
                return Ok(new { Message = "Password reset OTP sent to phone number." });
            }
            return NotFound(new { Message = "User not found." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to request password reset");
            return StatusCode(500, new { Message = $"Failed to request password reset: {ex.Message}" });
        }
    }

    [HttpPost("confirm-password-reset")]
    public async Task<IActionResult> ConfirmPasswordReset(ConfirmResetPasswordRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (await _userService.ConfirmPasswordReset(request))
            {
                await _producerKafkaService.Produce("PasswordResetConfirmed", request.PhoneNumber);
                return Ok(new { Message = "Password reset successfully." });
            }
            return BadRequest(new { Message = "Invalid OTP or OTP expired." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to confirm password reset");
            return StatusCode(500, new { Message = $"Failed to confirm password reset: {ex.Message}" });
        }
    }
}



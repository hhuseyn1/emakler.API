﻿using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.UserServices;
using DTO.User;
using EMakler.PROAPI.Configurations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
    private readonly IOtpService _otpService;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IUserService userService,
        IOtpService otpService,
        IOptions<JwtSettings> jwtSettings,
        ILogger<AuthController> logger)
    {
        _userService = userService;
        _otpService = otpService;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(UserRegistration userRegistration)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _userService.RegisterUser(userRegistration);
            _logger.LogInformation($"User registered successfully with email: {userRegistration.Email}");
            return Ok(new { Message = "User registered successfully. OTP sent to phone number." });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "User registration failed due to argument exception.");
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "User registration failed due to an unexpected exception.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpPost("Verify-Otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var isValid = await _otpService.VerifyOtpAsync(request);
            if (isValid)
            {
                _logger.LogInformation($"OTP verified successfully for contact number: {request.ContactNumber}");
                return Ok(new { Message = "OTP verified successfully." });
            }
            else
            {
                _logger.LogWarning($"OTP verification failed for contact number: {request.ContactNumber}");
                return BadRequest(new { Message = "Invalid OTP or OTP expired." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OTP verification failed due to an unexpected exception.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(UserLoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var isValidUser = await _userService.ValidateUserAsync(loginRequest.Email, loginRequest.Password);
            if (!isValidUser)
            {
                _logger.LogWarning($"Login attempt failed for user: {loginRequest.Email}");
                return Unauthorized(new { Message = "Invalid login attempt." });
            }

            var token = GenerateJwtToken(loginRequest.Email);
            _logger.LogInformation($"User logged in successfully with email: {loginRequest.Email}");

            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed due to an unexpected exception.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    private string GenerateJwtToken(string email)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(10),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { message = "Logged out successfully" });
    }
}

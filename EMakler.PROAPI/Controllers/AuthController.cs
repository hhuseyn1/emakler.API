using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interfaces.AuthService;
using DTO.Auth;
using FluentValidation;
using Serilog;

namespace EMakler.PROAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
    {
        try
        {
            var userDto = await _authService.RegisterUserAsync(request);
            Log.Information($"User registered successfully: {userDto}");
            return Ok(userDto);
        }
        catch (ValidationException ex)
        {
            Log.Warning(ex, $"Registration validation failed for contact number: {request.ContactNumber}. Errors: {string.Join(", ", ex.Errors)}");
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Unexpected error during registration for contact number: {request.ContactNumber}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        try
        {
            var authResponse = await _authService.LoginUserAsync(request);
            Log.Information($"Login successful for contact number: {request.ContactNumber}. Token generated.");
            return Ok(authResponse);
        }
        catch (UnauthorizedAccessException ex)
        {
            Log.Warning(ex, $"Login failed for contact number: {request.ContactNumber}");
            return Unauthorized("Invalid credentials");
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Unexpected error during login for email: {request.ContactNumber}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
}

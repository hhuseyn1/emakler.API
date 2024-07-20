using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interfaces.AuthService;
using DTO.Auth;
using FluentValidation;

namespace EMakler.PROAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
    {
        try
        {
            var userDto = await _authService.RegisterUserAsync(request);
            _logger.LogInformation($"User registered successfully: {userDto}");
            return Ok(userDto);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, $"Registration validation failed for email: {request.Email}. Errors: {string.Join(", ", ex.Errors)}");
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unexpected error during registration for email: {request.Email}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        try
        {
            var token = await _authService.LoginUserAsync(request);
            _logger.LogInformation($"Login successful for email: {request.Email}. Token generated.");
            return Ok(new { Token = token });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, $"Login failed for email: {request.Email}");
            return Unauthorized("Invalid credentials");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unexpected error during login for email: {request.Email}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }
}

using BusinessLayer.Interfaces.AuthService;
using DTO.Auth;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EMakler.PROAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
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
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var user = await _authService.RegisterUserAsync(request);
            return Ok(user);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred during registration.");
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var authResponse = await _authService.LoginUserAsync(request);
            return Ok(authResponse);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred during login.");
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            return BadRequest("Refresh token is required.");
        }

        try
        {
            var newAccessToken = await _authService.RefreshTokenAsync(refreshToken);
            return Ok(new { AccessToken = newAccessToken });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred during token refresh.");
            return BadRequest(ex.Message);
        }
    }
}

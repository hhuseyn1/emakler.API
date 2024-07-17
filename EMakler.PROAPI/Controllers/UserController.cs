using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interfaces;
using DTO.User;

namespace EMakler.PROAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegistration userRegistration)
    {
        try
        {
            await _userService.RegisterUser(userRegistration);
            return Ok("User registered successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRequest loginRequest)
    {
        try
        {
            var isValidUser = await _userService.ValidateUserAsync(loginRequest.Email, loginRequest.Password);
            if (!isValidUser)
            {
                return Unauthorized("Invalid email or password.");
            }
            return Ok("User logged in successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging in user.");
            return StatusCode(500, "Internal server error.");
        }
    }
}

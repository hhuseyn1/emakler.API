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


    [HttpPost("{userId}/Confirm-email")]
    public async Task<IActionResult> ConfirmEmail(Guid userId)
    {
        try
        {
            await _userService.ConfirmEmailAsync(userId);
            return Ok("Email confirmed successfully.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        try
        {
            await _userService.ChangePasswordAsync(request);
            return Ok(new { message = "Password changed successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(Guid userId, UpdateUserDto updateUserDto)
    {
        await _userService.UpdateUserAsync(userId, updateUserDto);
        return Ok(new { Message = "User updated successfully." });
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await _userService.DeleteUserAsync(userId);
        return Ok(new { Message = "User deleted successfully." });
    }

}

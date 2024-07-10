using BusinessLayer.Interfaces;
using DTO.User;
using Microsoft.AspNetCore.Mvc;

namespace EMakler.PROAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistration userRegistration)
    {
        try
        {
            await _userService.RegisterUser(userRegistration);
            return Ok(new { message = "User registered successfully." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPost("{userId}/confirm-email")]
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

    [HttpPost("changePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        try
        {
            await _userService.ChangePasswordAsync(request);
            return Ok(new { message = "Password changed successfully." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request)
    {
        try
        {
            var result = await _userService.SendOtpAsync(request);
            if (result)
            {
                return Ok(new { message = "OTP sent successfully." });
            }
            return BadRequest(new { message = "Failed to send OTP." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
    {
        try
        {
            var result = await _userService.VerifyOtpAsync(request);
            if (result)
            {
                return Ok(new { message = "OTP verified successfully." });
            }
            return BadRequest(new { message = "Invalid OTP or OTP expired." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        try
        {
            var userDto = await _userService.GetUserByIdAsync(userId);
            return Ok(userDto);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("by-mail/{userMail}")]
    public async Task<IActionResult> GetUserByMail(string userMail)
    {
        try
        {
            var userDto = await _userService.GetUserByMailAsync(userMail);
            return Ok(userDto);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("by-contact/{contactNumber}")]
    public async Task<IActionResult> GetUserByContactNumber(string contactNumber)
    {
        try
        {
            var userDto = await _userService.GetUserByContactNumberAsync(contactNumber);
            return Ok(userDto);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto updateUserDto)
    {
        try
        {
            await _userService.UpdateUserAsync(userId, updateUserDto);
            return Ok(new { message = "User updated successfully." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        try
        {
            await _userService.DeleteUserAsync(userId);
            return Ok(new { message = "User deleted successfully." });
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error.");
        }
    }
}

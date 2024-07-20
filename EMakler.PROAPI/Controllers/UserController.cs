using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces.UserServices;
using DTO.User;
using Microsoft.AspNetCore.Mvc;

namespace EMakler.PROAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("GetUserbyId/{userId}")]
    public async Task<IActionResult> GetUserbyId(Guid userId)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(userId);
            return Ok(user);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        var createdUser = await _userService.CreateUserAsync(createUserDto);
        return CreatedAtAction(nameof(GetUserbyId), new { userId = createdUser.Id }, createdUser);
    }

    [HttpPut("UpdateUserbyId/{userId}")]
    public async Task<IActionResult> UpdateUserbyId(Guid userId, [FromBody] UpdateUserDto updateUserDto)
    {
        if (userId != updateUserDto.Id)
            return BadRequest("User ID mismatch.");

        try
        {
            var updatedUser = await _userService.UpdateUserbyIdAsync(userId, updateUserDto);
            return Ok(updatedUser);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("DeleteUserbyId/{userId}")]
    public async Task<IActionResult> DeleteUserbyId(Guid userId)
    {
        try
        {
            await _userService.DeleteUserbyIdAsync(userId);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

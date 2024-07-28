using BusinessLayer.Interfaces.UserServices;
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

    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("GetUserById/{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return Ok(user);
    }

    [HttpGet("GetUserByContactNumber")]
    public async Task<IActionResult> GetUserByContactNumber([FromQuery] string contactNumber)
    {
        var user = await _userService.GetUserByContactNumberAsync(contactNumber);
        return Ok(user);
    }

    [HttpGet("GetUserByEmail")]
    public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
    {
        var user = await _userService.GetUserByEmailAsync(email);
        return Ok(user);
    }

    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
    {
        var createdUser = await _userService.CreateUserAsync(userDto);
        return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
    }

    [HttpPut("UpdateUserById/{id}")]
    public async Task<IActionResult> UpdateUserById(Guid id, [FromBody] UserDto userDto)
    {
        var updatedUser = await _userService.UpdateUserbyIdAsync(id, userDto);
        return Ok(updatedUser);
    }

    [HttpDelete("DeleteUserById/{id}")]
    public async Task<IActionResult> DeleteUserById(Guid id)
    {
        await _userService.DeleteUserbyIdAsync(id);
        return NoContent();
    }
}

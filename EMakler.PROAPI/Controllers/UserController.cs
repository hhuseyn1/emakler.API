using BusinessLayer.Interfaces;
using DTO.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMakler.PROAPI.Controllers
{
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
        public async Task<IActionResult> Register(UserRegsitration userRegsitration)
        {
            var result = await _userService.RegisterUser(userRegsitration);
            if (result)
            {
                return Ok(new { Message = "User registrated succesfully" });
            }

            return BadRequest(new { Message = "Invalid OTP or user registration failed" });
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] string phoneNumber)
        {
            var result=await _userService.SendOtp(phoneNumber);
            if (result)
            {
                return Ok(new { Message = "Otp sent to phone number." });
            }
            return BadRequest(new { Message = "Failed to sent Otp" });
        }

    }
}

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
        public async Task<IActionResult> Register(UserRegistration userRegistration)
        {
            await _userService.RegisterUser(userRegistration);
            return Ok(new { Message = "User registered successfully. OTP sent to phone number." });

        }

        #region comments
        //[HttpPost("register-validate")]
        //public async Task<IActionResult> RegisterAndValidate([FromBody] UserRegistrationWithOtp model)
        //{

        //    await _userService.RegisterUser(model.UserRegistration);

        //    string phoneNumber = model.UserRegistration.PhoneNumber;

        //    var isValid = await _userService.ValidateOtpAsync(phoneNumber, model.OtpCode);

        //    if (isValid)
        //    {
        //        return Ok(new { Message = "User registered and OTP validated successfully." });
        //    }
        //    return BadRequest(new { Message = "User registered but OTP validation failed." });
        //}
        // ----------------------------------
        //[HttpPost("validate-otp")]
        //public async Task<IActionResult> ValidateOtp(/*[FromBody]*/string phoneNumber, string otpCode)
        //{
        //    var isValid = await _userService.ValidateOtpAsync(phoneNumber, otpCode);
        //    if (isValid)
        //    {
        //        return Ok(new { Message = "OTP validated successfully." });
        //    }
        //    return BadRequest(new { Message = "Invalid OTP." });
        //}
        #endregion
    }
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateOtpController : ControllerBase
    {
        private readonly IUserService _userService;

        public ValidateOtpController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("validate-otp")]
        public async Task<IActionResult> ValidateOtp([FromBody] string phoneNumber, string otpCode)
        {
            var isValid = await _userService.ValidateOtpAsync(phoneNumber, otpCode);
            if (isValid)
            {
                return Ok(new { Message = "OTP validated successfully." });
            }
            return BadRequest(new { Message = "Invalid OTP." });
        }
    }
}

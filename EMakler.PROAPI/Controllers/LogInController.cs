using BusinessLayer.Interfaces;
using DTO.User;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EMakler.PROAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        private readonly IUserService _userService;
        public LogInController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogInModel userLogInModel)
        {
            if (string.IsNullOrEmpty(userLogInModel.PhoneNumber) || string.IsNullOrEmpty(userLogInModel.Password))
            {
                return BadRequest("Phone number and password are required.");
            }

            var isAuthenticated = await _userService.AuthenticateUserAsync(userLogInModel.PhoneNumber, userLogInModel.Password);
            if (!isAuthenticated)
            {
                return BadRequest("Invalid phone number or password.");
            }

            string token = GenerateJwtToken(userLogInModel.PhoneNumber); 
            return Ok("Welcome, Əmlak Bazası");
        }

        //[HttpPost("forgot-password")]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        //{
        //    if (string.IsNullOrEmpty(forgotPasswordModel.PhoneNumber))
        //    {
        //        return BadRequest("Phone number is required");
        //    }

        //    var otp = await _userService.GenerateOtpAsync(forgotPasswordModel.PhoneNumber);

        //    if (string.IsNullOrEmpty(otp))
        //    {
        //        return BadRequest("Failed to generate Otp");
        //    }

        //    bool isSent=await _userService.SendOtpAsync(forgotPasswordModel.PhoneNumber, otp);

        //    if (!isSent)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to send OTP.");
        //    }

        //    return Ok("OTP has been sent to your phone number.");
        //}


        //[HttpPost("reset-password")]
        //public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        //{
        //    if (string.IsNullOrEmpty(resetPasswordModel.PhoneNumber) || string.IsNullOrEmpty(resetPasswordModel.OtpCode) || string.IsNullOrEmpty(resetPasswordModel.NewPassword))
        //    {
        //        return BadRequest("Phone number, OTP code, and new password are required.");
        //    }

        //    //Validate Otp
        //    var isOtpValid = await _userService.ValidateOtpAsync(resetPasswordModel.PhoneNumber, resetPasswordModel.OtpCode);

        //    if (!isOtpValid)
        //    {
        //        return BadRequest("Invalid OTP code.");
        //    }

        //    //Reset Password
        //    var isPasswordReset = await _userService.ResetPasswordAsync(resetPasswordModel.PhoneNumber, resetPasswordModel.NewPassword);

        //    if (!isPasswordReset)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to reset password.");
        //    }

        //    return Ok("Password has been reset successfully.");
        //}

        #region private method
        private string GenerateJwtToken(string phoneNumber)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes("emaklerprosecretkeyouldnotcommit32byteslong!");

            var symmetricSecurityKey = new SymmetricSecurityKey(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(phoneNumber),
                Expires = DateTime.UtcNow.AddHours(1), // Set token expiration time
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        #endregion
    }
}

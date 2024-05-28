using BusinessLayer.Interfaces;
using DTO.User;
using EMakler.PROAPI.Utilities;
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
    }
}

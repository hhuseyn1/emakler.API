using DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IUserService
    {
        Task<bool> AuthenticateUserAsync(string phoneNumber, string password);
        
        Task<bool> ValidateOtpAsync(string phoneNumber, string otpCode);
        Task RegisterUser(UserRegistration userRegistration);
        //Task<bool> ResetPasswordAsync(string phoneNumber, string newPassword);
        //Task<string> GenerateOtpAsync(string phoneNumber);
        //Task<bool> SendOtpAsync(string phoneNumber, string otp);
    }
}

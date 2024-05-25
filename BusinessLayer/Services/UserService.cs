using Azure.Core;
using BusinessLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DTO.User;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterUser(UserRegsitration userRegsitration)
        {
            // Verify the OTP code
            var storedOtp = await _userRepository.GetOtpAsync(userRegsitration.PhoneNumber);
            if (storedOtp != userRegsitration.OtpCode)
            {
                return false; // Invalid OTP
            }

            // Save user to database
            var user = new User
            {
                UserMail = userRegsitration.Email,
                ContactNumber = userRegsitration.PhoneNumber,
                OtpCode = userRegsitration.OtpCode
            };

            await _userRepository.AddUserAsync(user);

            return true;
        }

        public async Task<bool> SendOtp(string phoneNumber)
        {
            var otpCode = GenerateOtp();
            await SendOtpAsync(phoneNumber, otpCode);

            // Save OTP to database
            await _userRepository.SaveOtpAsync(phoneNumber, otpCode);

            return true;
        }


        #region privatemethods
        private string GenerateOtp()
        {
            
            return new Random().Next(100000, 999999).ToString();
        }

        private async Task SendOtpAsync(string phoneNumber, string otpCode)
        {
            await Task.Run(() => {
                Console.WriteLine($"Sending OTP {otpCode} to phone number {phoneNumber}");
            });
        }
        #endregion
    }
}

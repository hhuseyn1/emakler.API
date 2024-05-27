using BusinessLayer.Interfaces;
using DataAccessLayer.Concrete;
using DataAccessLayer.Interfaces;
using DTO.User;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly Context _context;
        public UserService(Context context)
        {
            _context = context;
        }

        public async Task RegisterUser(UserRegistration userRegistration)
        {
            var otpCode = GenerateOtp();
            var user = new User
            {
                UserId = userRegistration.Id++,
                UserMail = userRegistration.Email,
                ContactNumber = userRegistration.PhoneNumber,
                OtpCode = otpCode
            };


            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateOtpAsync(string contactNumber, string otpCode)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ContactNumber == contactNumber);
            if (user != null && user.OtpCode == otpCode)
            {
                return true;
            }
            return false;
        }



        #region privatemethods
        private string GenerateOtp()
        {

            return new Random().Next(100000, 999999).ToString();
        }

        #endregion

    }
}

using BusinessLayer.Interfaces;
using DataAccessLayer.Concrete;
using DTO.User;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using OtpNet;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly Context _context;
        private readonly Totp _totp;
        public UserService(Context context)
        {
            _context = context;
            _totp = new Totp(Base32Encoding.ToBytes("emaklerprosecret"));
        }

        public async Task<bool> AuthenticateUserAsync(string phoneNumber, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ContactNumber == phoneNumber);

            if (user == null)
            {
                return false;
            }

            if (user.UserPassword == password)
            {
                return true; 
            }

            return false; 
        }

        public async Task RegisterUser(UserRegistration userRegistration)
        {
            var otpCode = GenerateOtp();
            var user = new User
            {

                UserMail = userRegistration.Email,
                ContactNumber = userRegistration.PhoneNumber,
                OtpCode = otpCode,
                OtpCreatedTime = DateTime.UtcNow,
                IsValidate=true
            };

            var previousCodes = await _context.Users.Where(x => x.ContactNumber == userRegistration.PhoneNumber && x.IsValidate).ToListAsync();
            foreach (var prevCodeUser in previousCodes)
            {
                prevCodeUser.IsValidate = false;
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateOtpAsync(string contactNumber, string otpCode)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ContactNumber == contactNumber&&x.IsValidate==true);
            if (user != null)
            {
                TimeSpan otpValidityDuration = TimeSpan.FromMinutes(5);

                if (DateTime.UtcNow - user.OtpCreatedTime <= otpValidityDuration)
                {
                    return true;
                }
            }

            return false;
        }



        #region privatemethod
        private string GenerateOtp()
        {
            return _totp.ComputeTotp();
        }

        #endregion

    }
}

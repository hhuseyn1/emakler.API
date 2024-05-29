using BusinessLayer.Interfaces;
using DataAccessLayer.Concrete;
using DTO.User;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using OtpNet;
using System.Security.Cryptography;
using System.Text;

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

            if (VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return true;
            }

            return false; 
        }

        //public async Task<string> GenerateOtpAsync(string phoneNumber)
        //{
        //    var otp = GenerateOtp();
        //    var user = await _context.Users.FirstOrDefaultAsync(u => u.ContactNumber == phoneNumber);

        //    if (user != null)
        //    {
        //        user.OtpCode = otp;
        //        user.OtpCreatedTime = DateTime.UtcNow;
        //        await _context.SaveChangesAsync();
        //        return otp;
        //    }

        //    return null;
        //}

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

        //public async Task<bool> ResetPasswordAsync(string phoneNumber, string newPassword)
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(u => u.ContactNumber == phoneNumber);
        //    if (user == null)
        //    {
        //        return false;
        //    }

        //    using var hmac = new HMACSHA512();
        //    user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
        //    user.PasswordSalt = hmac.Key;

        //    _context.Users.Update(user);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        //public Task<bool> SendOtpAsync(string phoneNumber, string otp)
        //{
        //    throw new NotImplementedException();
        //}

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

        //private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        //{
        //    using (var hmac = new HMACSHA512(storedSalt))
        //    {
        //        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        //        return computedHash.SequenceEqual(storedHash);
        //    }
        //}

        #endregion

    }
}

﻿using DataAccessLayer.Concrete;
using DataAccessLayer.Interfaces;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;

        public UserRepository(Context context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetOtpAsync(string phoneNumber)
        {
            var otpEntity = await _context.Users.Where(x => x.ContactNumber == phoneNumber)
                .OrderByDescending(o => o.UserId)
                .FirstOrDefaultAsync();

            return otpEntity?.OtpCode;
        }

        public async Task SaveOtpAsync(string phoneNumber, string otpCode)
        {
            var user = new User
            {
                UserId = 1,
                ContactNumber = phoneNumber,
                OtpCode = otpCode,
                UserMail = "",
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

    }
}
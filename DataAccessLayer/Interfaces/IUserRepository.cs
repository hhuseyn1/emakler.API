using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task SaveOtpAsync(string phoneNumber, string otpCode);
        Task<string> GetOtpAsync(string phoneNumber);
    }
}

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
        Task<bool> RegisterUser(UserRegsitration userRegsitration);
        Task<bool> SendOtp(string phoneNumber);
    }
}

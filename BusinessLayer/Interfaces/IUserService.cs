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
        Task RegisterUser(UserRegistration userRegsitration);
        Task<bool> ValidateOtpAsync(string contactNumber, string otpCode);

    }
}

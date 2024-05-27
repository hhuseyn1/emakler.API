using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.User
{
    public class UserRegistrationWithOtp
    {
        public UserRegistration UserRegistration { get; set; }
        public string OtpCode {  get; set; }
    }
}

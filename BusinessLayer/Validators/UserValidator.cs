using EntityLayer.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Validators
{
    public class UserValidator:AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.ContactNumber)
                .NotEmpty()
                .WithMessage("Phone number cannot be empty");

            RuleFor(x => x.UserMail)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Email is incorrect");

            //RuleFor(x => x.UserPassword)
            //    .NotEmpty()
            //    .MinimumLength(8).When(x => x.UserPassword.Length > 0)
            //    .WithMessage("Password must be longer than 8 characters");


                     
        }
    }
}

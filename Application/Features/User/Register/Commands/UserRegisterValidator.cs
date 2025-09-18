using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.User;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.User.Register.Commands
{
    public class UserRegisterValidator : AbstractValidator<RegisterDto>
    {
        private readonly string email;
        private readonly UserManager<Domain.Entities.User> _userManager;

        public UserRegisterValidator(string email, UserManager<Domain.Entities.User> userManager)
        {
            this.email = email;
            _userManager = userManager;

            RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("Email is required")
            .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
            .WithMessage("Invalid email format.")
            .Must(IsEmailTaken).WithMessage("Email is already taken.");


            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("Password is required")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d")
                .WithMessage("Password must contain at least one number.")
                .Matches(@"[\W_]")
                .WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.ConfirmPassword)
               .NotNull()
               .NotEmpty()
               .WithMessage("Confirm Password is required")
               .When(x => !x.ConfirmPassword.Equals(x.Password))
               .WithMessage("Passwords don't match")
               .MinimumLength(8)
               .WithMessage("Password must be at least 8 characters long.")
               .Matches(@"[A-Z]")
               .WithMessage("Password must contain at least one uppercase letter.")
               .Matches(@"[a-z]")
               .WithMessage("Password must contain at least one lowercase letter.")
               .Matches(@"\d")
               .WithMessage("Password must contain at least one number.")
               .Matches(@"[\W_]")
               .WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.PassportNumber)
                .NotNull()
                .NotEmpty()
                .MinimumLength(8)
                .WithMessage("Passport number must have 8 numbers at least");

            RuleFor(x => x.PhoneNumber)
                .NotNull()
                .NotEmpty()
                .MinimumLength(11)
                .WithMessage("Phone number number must have 11 numbers");
        }

        private bool IsEmailTaken(string email)
        {
            var user = _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return true;
            }
            return false;
        }
    }
}

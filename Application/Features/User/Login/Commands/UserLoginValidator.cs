﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.User;
using FluentValidation;

namespace Application.Features.User.Login.Commands
{
    public class UserLoginValidator : AbstractValidator<LoginDto>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.Email)
              .NotNull()
              .NotEmpty()
              .WithMessage("Email is required")
              .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
              .WithMessage("Invalid email ");

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("Password is required");
        }
    }
}

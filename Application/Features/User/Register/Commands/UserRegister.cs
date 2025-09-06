using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Dtos.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.User.Register.Commands
{
    public class UserRegister
    {
        private readonly UserManager<Domain.User> userManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;

        public UserRegister(UserManager<Domain.User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<BaseResponse<Guid>> Register(RegisterDto registerDto)
        {
            var response = new BaseResponse<Guid>();
            response.Message = null;
            response.Data = Guid.Empty;

            var validator = new UserRegisterValidator(registerDto.Email, userManager);
            var result = validator.Validate(registerDto);
            if (!result.IsValid)
            {
                response.Message = string.Join(",", result.Errors.Select(x => x.ErrorMessage));
                return response;
            }
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == registerDto.Email);
            if (user != null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "This user is already registered";
                return response;
            }


            var User = new Domain.User
            {
                Id = Guid.NewGuid(),
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Email.Split("@")[0],
                Nationality = registerDto.Nationality,
                PassportNumber = registerDto.PassportNumber,
                CreatedAt = DateTime.Now,
                PhoneNumber = registerDto.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            bool IsFirstUser = !userManager.Users.Any();
            var check = await userManager.CreateAsync(User, registerDto.Password);
            if (IsFirstUser)
            {
                await userManager.AddToRoleAsync(User, "SuperAdmin");
            }
            else
            {
                await userManager.AddToRoleAsync(User, "User");

            }
            if (!check.Succeeded)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = $"Registration failed : {string.Join(",", check.Errors.Select(x => x.Description))}";
                return response;
            }
            response.StatusCode = HttpStatusCode.OK;
            response.Message = "Registration Successfull";
            return response;
        }
    }
}

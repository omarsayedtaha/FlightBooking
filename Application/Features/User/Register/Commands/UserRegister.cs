using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CommonDefenitions;
using CommonDefenitions.Dtos.User;
using Infrastructure;
using Microsoft.AspNetCore.Identity;

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
            response.StatusCode = HttpStatusCode.OK;
            response.Message = null;
            response.Data = Guid.Empty;

            var validator = new UserRegisterValidator();
            var result = validator.Validate(registerDto);
            if (!result.IsValid)
            {
                response.Message = string.Join(",", result.Errors.Select(x => x.ErrorMessage));
                return response;
            }
            var user = userManager.Users.FirstOrDefault(x => x.Email == registerDto.Email);
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
            var check = await userManager.CreateAsync(User, registerDto.Password);

            if (registerDto.IsAdmin)
            {
                await userManager.AddToRoleAsync(User, "Admin");
            }
            else
            {
                await userManager.AddToRoleAsync(User, "User");
            }

            if (check.Succeeded)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Registration Successfull";
                response.Data = User.Id;
                return response;
            }

            response.Message = string.Join(",", check.Errors.Select(x => x.Description));
            return response;
        }
    }
}

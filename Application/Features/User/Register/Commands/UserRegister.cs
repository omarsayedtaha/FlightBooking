using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Dtos.User;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Application.Features.User.Register.Commands
{
    public class UserRegister
    {
        private readonly UserManager<Domain.Entities.User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IApplicationDbContext _dbContext;

        public UserRegister(UserManager<Domain.Entities.User> userManager,
            RoleManager<IdentityRole> roleManager)
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

            var User = new Domain.Entities.User
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Email.Split("@")[0],
                Nationality = registerDto.Nationality,
                PassportNumber = registerDto.PassportNumber,
                CreatedAt = DateTime.Now,
                PhoneNumber = registerDto.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var check = new IdentityResult();
            if (!userManager.Users.Any())
            {
                User.IsAdmin = true;
                check = await userManager.CreateAsync(User, registerDto.Password);
                if (!await roleManager.RoleExistsAsync("Admin"))
                    await roleManager.CreateAsync(new IdentityRole("Admin"));

                await userManager.AddToRoleAsync(User, "Admin");

            }
            else
            {
                check = await userManager.CreateAsync(User, registerDto.Password);
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

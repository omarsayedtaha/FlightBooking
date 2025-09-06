using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Dtos.User;
using Application.Features.User.Register;
using Domain;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Features.User.Login.Commands
{
    public class UserLogin
    {
        private readonly UserManager<Domain.User> userManager;
        private readonly SignInManager<Domain.User> signInManager;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserLogin(UserManager<Domain.User> userManager
            , SignInManager<Domain.User> signInManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        public async Task<BaseResponse<UserDto>> Login(LoginDto loginmodel)
        {
            var response = new BaseResponse<UserDto>();
            response.Message = null;
            response.Data = null;

            var validator = new UserLoginValidator();
            var result = validator.Validate(loginmodel);
            if (!result.IsValid)
            {
                response.Message = string.Join(",", result.Errors.Select(x => x.ErrorMessage));
                return response;
            }
            var user = userManager.Users.FirstOrDefault(x => x.Email == loginmodel.Email);
            if (user == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Login failed";
                return response;
            }
            var userResult = await signInManager.PasswordSignInAsync(user, loginmodel.Password, false, false);
            if (userResult.IsNotAllowed)
            {
                response.Message = "Login failed";
                return response;
            }
            //Update user table to add token 
            user.Token = await GenerateToken(user);
            // user.RefreshToken = Guid.NewGuid();
            // user.RefreshTokenExpiryDate = DateTime.Now.AddMonths(2);
            await userManager.UpdateAsync(user);

            response.StatusCode = HttpStatusCode.OK;
            response.Message = "Welcome";
            response.Data = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Token = user.Token,
                // RefreshToken = user.RefreshToken,

            };
            return response;
        }

        public async Task<string> GenerateToken(Domain.User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid,user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.UserName),
                new Claim(ClaimTypes.Name,$"{user.FirstName} {user.LastName}"),
            };
            var Roles = await userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMonths(1),
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(tokenDescriptor);
        }

    }
}

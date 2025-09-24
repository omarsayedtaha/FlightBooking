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
        private readonly UserManager<Domain.Entities.User> userManager;
        private readonly SignInManager<Domain.Entities.User> signInManager;
        private readonly IConfiguration configuration;
        private readonly ITokenService tokenService;

        public UserLogin(UserManager<Domain.Entities.User> userManager
            , SignInManager<Domain.Entities.User> signInManager,
            IConfiguration configuration, ITokenService tokenService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.tokenService = tokenService;
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
            user.Token = await tokenService.GenerateToken(user);
            user.RefreshToken = Guid.NewGuid().ToString();
            user.RefreshTokenExpiryDate = DateTime.Now.AddMonths(1);
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
                RefreshToken = user.RefreshToken,

            };
            return response;
        }

    }
}

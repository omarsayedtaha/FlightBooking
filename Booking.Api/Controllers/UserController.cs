using System.Threading.Tasks;
using Application.Common;
using Application.Dtos.User;
using Application.Features.User.Login;
using Application.Features.User.Login.Commands;
using Application.Features.User.Register;
using Application.Features.User.Register.Commands;
using Application.Interfaces;
using Domain;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ess;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRegister userRegister;
        private readonly UserLogin userLogin;
        private readonly ForgetPassword forgetPassword;
        private readonly UserLogout userLogout;
        private readonly RefreshUserToken refreshUserToken;

        public UserController(
            UserRegister userRegister,
            UserLogin userLogin,
            ForgetPassword forgetPassword,
            UserLogout userLogout,
            RefreshUserToken refreshUserToken)
        {
            this.userRegister = userRegister;
            this.userLogin = userLogin;
            this.forgetPassword = forgetPassword;
            this.userLogout = userLogout;
            this.refreshUserToken = refreshUserToken;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var User = await userRegister.Register(registerDto);

            return Ok(User);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {

            var User = await userLogin.Login(loginDto);

            return Ok(User);
        }

        [AllowAnonymous]
        [HttpGet("forgetPassword/{email}")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var result = await forgetPassword.SendOTP(email);
            return Ok(result);

        }

        [AllowAnonymous]
        [HttpPut("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var result = await forgetPassword.ResetPassword(resetPasswordDto.otp, resetPasswordDto.Password);
            return Ok(result);

        }
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await userLogout.Logout();
            return Ok();
        }

        [Authorize]
        [HttpPost("refresh-user-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshUserTokenDto refreshUserTokenDto)
        {
            return Ok(await refreshUserToken.RefreshToken(refreshUserTokenDto));

        }
    }
}

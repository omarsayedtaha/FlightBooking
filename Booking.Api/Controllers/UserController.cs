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

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IApplicationDbContext applicationDbContext;
        private readonly IConfiguration configuration;
        private readonly SignInManager<User> signInManager;
        private readonly IMailService mailService;
        private readonly AppHelperSerivices appHelper;

        public UserController(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IApplicationDbContext applicationDbContext, IConfiguration configuration
            , SignInManager<User> signInManager, IMailService mailService, AppHelperSerivices appHelper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.applicationDbContext = applicationDbContext;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.mailService = mailService;
            this.appHelper = appHelper;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var Service = new UserRegister(userManager, roleManager);

            var User = await Service.Register(registerDto);

            return Ok(User);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var Service = new UserLogin(userManager, signInManager, configuration);

            var User = await Service.Login(loginDto);

            return Ok(User);
        }

        [AllowAnonymous]
        [HttpGet("forgetPassword/{email}")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var service = new ForgetPassword(userManager, mailService, configuration);
            var result = await service.SendOTP(email);
            return Ok(result);

        }

        [AllowAnonymous]
        [HttpPut("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var service = new ForgetPassword(userManager, mailService, configuration);
            var result = await service.ResetPassword(resetPasswordDto.otp, resetPasswordDto.Password);
            return Ok(result);

        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = await appHelper.GetUserIdAsync();
            var user = await userManager.FindByIdAsync(userId.ToString());
            user.Token = "";
            await applicationDbContext.SaveChangesAsync();
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await signInManager.SignOutAsync();
            return Ok("Logged out successfully");
        }
    }
}

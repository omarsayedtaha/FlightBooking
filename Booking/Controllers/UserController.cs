using Application.Features.User.Login;
using Application.Features.User.Login.Commands;
using Application.Features.User.Register;
using Application.Features.User.Register.Commands;
using CommonDefenitions.Dtos.User;
using Domain;
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
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IConfiguration configuration;
        private readonly SignInManager<User> signInManager;

        public UserController(UserManager<User>userManager,
            RoleManager<IdentityRole<Guid>>roleManager,
            ApplicationDbContext applicationDbContext,IConfiguration configuration
            ,SignInManager<User>signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.applicationDbContext = applicationDbContext;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }
        [HttpPost("register")]   
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var Service = new UserRegister(userManager, roleManager);

            var User =await Service.Register(registerDto);

            return Ok(User);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var Service = new UserLogin(userManager,signInManager,configuration);

            var User = await Service.Login(loginDto);

            return Ok(User);
        }

    }
}

using Application.Common;
using Application.Common.services;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

public class UserLogout
{
    private readonly IApplicationDbContext dbContext;
    private readonly AppHelperSerivices appHelper;
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ILogger<UserLogout> logger;

    public UserLogout(IApplicationDbContext dbContext,
        AppHelperSerivices appHelper,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IHttpContextAccessor httpContextAccessor,
        ILogger<UserLogout> logger)
    {
        this.dbContext = dbContext;
        this.appHelper = appHelper;
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.httpContextAccessor = httpContextAccessor;
        this.logger = logger;
    }

    public async Task Logout()
    {
        var user = await appHelper.GetUserAsync();
        user.Token = "";
        user.RefreshToken = "";
        user.RefreshTokenExpiryDate = null;
        await dbContext.SaveChangesAsync();
        string token = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        await signInManager.SignOutAsync();
        logger.LogInformation($"user {user.UserName} has logged out at {DateTime.Now}");
    }
}
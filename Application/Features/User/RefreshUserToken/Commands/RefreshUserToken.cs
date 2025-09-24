using System.Net;
using Application.Common;
using Application.Common.services;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

public class RefreshUserToken
{
    private readonly AppHelperSerivices appHelper;
    private readonly IApplicationDbContext dbContext;
    private readonly IConfiguration config;
    private readonly ITokenService tokenService;
    private readonly UserManager<User> userManager;
    private readonly UserLogout userLogout;

    public RefreshUserToken(AppHelperSerivices appHelper,
     IApplicationDbContext dbContext,
      IConfiguration config,
       ITokenService tokenService,
       UserManager<User> userManager, UserLogout userLogout)
    {
        this.appHelper = appHelper;
        this.dbContext = dbContext;
        this.config = config;
        this.tokenService = tokenService;
        this.userManager = userManager;
        this.userLogout = userLogout;
    }

    public async Task<BaseResponse<RefreshUserTokenDto>> RefreshToken(RefreshUserTokenDto model)
    {

        var validator = new RefreshUserTokenValidator();
        var result = validator.Validate(model);
        if (!result.IsValid)
        {
            return
            new BaseResponse<RefreshUserTokenDto>(HttpStatusCode.BadRequest,
             $"Invaild Token \n {string.Join(",", result.Errors.Select(x => x.ErrorMessage))}", null);
        }

        var user = await appHelper.GetUserAsync();

        if (!user.RefreshToken.Contains(model.RefreshToken))
            return new BaseResponse<RefreshUserTokenDto>(HttpStatusCode.BadRequest, " Refresh Token not matching", null);

        if (!user.Token.Contains(model.AccessToken))
            return new BaseResponse<RefreshUserTokenDto>(HttpStatusCode.BadRequest, "Access Token not matching", null);

        if (user.RefreshTokenExpiryDate < DateTime.Now)
        {
            await userLogout.Logout();
            return new BaseResponse<RefreshUserTokenDto>(HttpStatusCode.OK, "you need to login", null);

        }

        user.Token = await tokenService.GenerateToken(user);
        user.RefreshToken = Guid.NewGuid().ToString();
        user.RefreshTokenExpiryDate = DateTime.Now.AddMonths(1);
        await userManager.UpdateAsync(user);
        await dbContext.SaveChangesAsync();

        var tokenDto = new RefreshUserTokenDto
        {
            AccessToken = user.Token,
            RefreshToken = user.RefreshToken
        };

        return new BaseResponse<RefreshUserTokenDto>(HttpStatusCode.OK, "Token Updated Successfully", tokenDto);
    }
}
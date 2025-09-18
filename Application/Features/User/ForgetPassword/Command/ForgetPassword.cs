using System.Net;
using System.Security.Cryptography;
using Application.Common;
using Application.Interfaces;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

public class ForgetPassword
{
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;
    private readonly IMailService mailSender;
    private readonly IConfiguration config;
    private readonly AppHelperSerivices appHelper;

    public ForgetPassword(UserManager<User> userManager,
    IMailService mailSender,
    IConfiguration config)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.mailSender = mailSender;
        this.config = config;
    }

    public async Task<BaseResponse<string>> SendOTP(string email)
    {

        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
            return new BaseResponse<string>(System.Net.HttpStatusCode.NotFound, "User not found", string.Empty);

        var Otp = new Otp();
        Otp.Code = new Random().Next(10000, 99999).ToString();
        var Recepient = email;
        var Body = $"This is your Otp {Otp.Code} and It will expire after 5 minuites";
        mailSender.SendEmail(Recepient, Body);
        StaticData.UserOtps.Add(user.Email, Otp);


        return new BaseResponse<string>(HttpStatusCode.OK, "Check your email", string.Empty);
    }

    public async Task<BaseResponse<string>> ResetPassword(string otpRequest, string Password)
    {
        if (StaticData.UserOtps == null)
            return new BaseResponse<string>(HttpStatusCode.NotFound, "No otp found", string.Empty);

        var otp = StaticData.UserOtps.Values.FirstOrDefault(o => o.Code == otpRequest);

        if (!otp.IsValid() || otp.IsUsed)
            return new BaseResponse<string>(HttpStatusCode.BadRequest, "InValid Otp", string.Empty);

        var useremail = StaticData.UserOtps.FirstOrDefault(x => x.Value == otp).Key;
        var user = await userManager.FindByEmailAsync(useremail);
        if (user == null)
            return new BaseResponse<string>(HttpStatusCode.NotFound, "User not found", string.Empty);

        await userManager.RemovePasswordAsync(user);

        await userManager.AddPasswordAsync(user, Password);
        otp.IsUsed = true;

        return new BaseResponse<string>(HttpStatusCode.OK, "Password has been reset successfully", string.Empty);

    }
}
using Application.Common;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

public class RefreshUserTokenValidator : AbstractValidator<RefreshUserTokenDto>
{
    public RefreshUserTokenValidator()
    {
        RuleFor(x => x.RefreshToken)
       .NotNull()
       .NotEmpty()
       .WithMessage("You must provide  refresh token");

        RuleFor(x => x.AccessToken)
     .NotNull()
     .NotEmpty()
     .WithMessage("You must provide  access token");


    }





}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using CommonDefenitions;
using Domain;
using Infrastructure;
using MailKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using IMailService = Application.Interfaces.IMailService;
using IApplicationDbContext = Application.Interfaces.IApplicationDbContext;
using Domain.Entities;
using Application.Features.User.Register.Commands;
using Application.Features.User.Login.Commands;
using Application.Common.services;
using Application.Features.Flight.Commands.Create;
using Application.Features.Flight.Commands.Update;
using Application.Features.Flight.Queries;
using Application.Features.FlightBooking.Commands.Update;
using Application.Features.Booking.Queries;
using Application.Features.Booking.Commands.Delete;


namespace Booking.Helper
{
    public static class Services
    {
        public static void AddServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;

            })
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"])),
                };
            });

            // services.AddCors(options =>
            // {
            //     options.AddPolicy("MyCorsPolicy", policy =>
            //     {
            //         policy.AllowAnyOrigin()
            //               .AllowAnyMethod()
            //               .AllowAnyHeader();
            //     });
            // });

            services.Configure<MailSettings>(config.GetSection(MailSettings.MailOptionsKey));
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<IMailService, EmailService>();
            services.AddScoped<BaseRequest>();
            services.AddScoped<AppHelperSerivices>();
            services.AddHttpContextAccessor();

            services.AddScoped<UserRegister>();
            services.AddScoped<UserLogin>();
            services.AddScoped<ForgetPassword>();
            services.AddScoped<UserLogout>();
            services.AddScoped<RefreshUserToken>();

            services.AddScoped<CreateFlight>();
            services.AddScoped<UpdateFlight>();
            services.AddScoped<GetFlightsWithFilter>();
            services.AddScoped<GetFlightWithId>();

            services.AddScoped<BookSeat>();
            services.AddScoped<ConfirmBooking>();
            services.AddScoped<UpdateBookingStatus>();
            services.AddScoped<GetUserBookings>();
            services.AddScoped<DeleteBooking>();

            services.AddScoped<ITokenService, TokenService>();



        }
    }
}

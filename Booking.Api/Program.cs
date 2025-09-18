
using System;
using System.Drawing;
using System.Text;
using Application.Interfaces;
using Booking.Helper;
using CommonDefenitions.Helper;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;

namespace Bookings
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddServices(builder.Configuration);
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            var app = builder.Build();

            var servicescope = app.Services.GetRequiredService<IServiceProvider>().CreateScope();
            var context = servicescope.ServiceProvider.GetService<ApplicationDbContext>();
            var loggerfactory = servicescope.ServiceProvider.GetService<ILoggerFactory>();
            var logger = loggerfactory.CreateLogger<Program>();

            try
            {
                var Database = context.Database;
                await Database.MigrateAsync();
                DataSeeding.SeedData(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // else
            // {
            //     app.UseSwagger();
            //     app.UseSwaggerUI(c =>
            //     {
            //         c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            //         c.RoutePrefix = string.Empty;
            //     });
            // }


            app.UseHttpsRedirection();

            // app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

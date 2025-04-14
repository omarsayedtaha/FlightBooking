
using System;
using System.Text;
using Booking.Helper;
using CommonDefenitions.Helper;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
            builder.Services.AddSwaggerGen();

            Services.AddServices(builder.Services, builder.Configuration);

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

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

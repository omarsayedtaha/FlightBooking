using Domain;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDbContext:IdentityDbContext<User,IdentityRole<Guid>,Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public ApplicationDbContext()
        {

        }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<FlightBookings> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            builder.Entity<IdentityRole<Guid>>()
                .HasData(new List<IdentityRole<Guid>>
                {
                    new IdentityRole<Guid> {Id=Guid.NewGuid(),Name ="Admin" , NormalizedName = "Admin" },
                    new IdentityRole<Guid> {Id=Guid.NewGuid() ,Name ="User" , NormalizedName = "User" }
                });
             

            
        }
    }
}

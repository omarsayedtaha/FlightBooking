using Application.Interfaces;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDbContext
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<FlightBookings> FlightBookings { get; set; }
        public DbSet<FlightSeatClass> FlightSeatClass { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Seat> Seats { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");


            builder.Entity<FlightBookings>()
            .HasOne(fb => fb.Seat)
            .WithOne(s => s.FlightBookings)
            .HasForeignKey<FlightBookings>(fb => fb.SeatId)
            .OnDelete(DeleteBehavior.NoAction); // no cascade from Seat

            builder.Entity<FlightBookings>()
                .HasOne(fb => fb.Flight)
                .WithMany(f => f.FlightBookings)
                .HasForeignKey(fb => fb.FlightId)
                .OnDelete(DeleteBehavior.NoAction); // no cascade from Flight

            builder.Entity<FlightBookings>()
                .HasOne(fb => fb.Booking)
                .WithMany(b => b.FlightBookings)
                .HasForeignKey(fb => fb.BookingId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FlightBookings>()
            .HasIndex(f => f.SeatId)
            .IsUnique();

            builder.Entity<Seat>()
            .HasOne(s => s.SeatClass)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        }


    }
}

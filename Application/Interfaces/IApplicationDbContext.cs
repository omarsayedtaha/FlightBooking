using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Flight> Flights { get; set; }
        public DbSet<FlightBookings> FlightBookings { get; set; }
        public DbSet<FlightSeatClass> FlightSeatClass { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Seat> Seats { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

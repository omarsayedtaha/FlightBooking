using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class FlightBookings
    {
        public Guid Id { get; set; }
        public Guid FlightId { get; set; }
        public Guid UserId { get; set; }
        public int NumberOfBookedSeats { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime TravelDate { get; set; }
        public string Status { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime? CancellationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public User User { get; set; }
        public Flight Flight { get; set; }
    }
}

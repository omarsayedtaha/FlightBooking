using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FlightBookings
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public int BookingId { get; set; }
        public int SeatId { get; set; }
        public string PassengerName { get; set; }
        public string PassportNumber { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public Flight Flight { get; set; }
        public Booking Booking { get; set; }
        public Seat Seat { get; set; }


    }
}

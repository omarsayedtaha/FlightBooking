using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Bookings
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public string UserId { get; set; }
        public int NumberOfBookedSeats { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime TravelDate { get; set; }
        public string Status { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime? CancellationDate { get; set; }

    }
}

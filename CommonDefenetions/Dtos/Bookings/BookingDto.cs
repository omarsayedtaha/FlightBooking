using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDefenitions.Dtos.Bookings
{
    public class BookingDto
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


    }
}

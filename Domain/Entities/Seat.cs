using Domian.Enums;

namespace Domain.Entities
{
    public class Seat
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; }
        public int FlightId { get; set; }
        public int FlightSeatClassId { get; set; }
        public bool IsBooked { get; set; }
        public Flight Flight { get; set; }
        public FlightBookings? FlightBookings { get; set; }
        public FlightSeatClass SeatClass { get; set; }

    }
}
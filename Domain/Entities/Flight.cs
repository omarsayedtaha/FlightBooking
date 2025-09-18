using Domian.Enums;

namespace Domain.Entities
{
    public class Flight
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public string Airline { get; set; }
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public bool HasArrived { get; set; } = false;
        public int NumberOfSeats { get; set; }
        public int NumberOfSeatsAvialable { get; set; }
        public int NumberOfBookedSeats { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<Seat> Seats { get; set; } = new List<Seat>();

        public IEnumerable<FlightBookings> FlightBookings { get; set; } = new List<FlightBookings>();

        public IEnumerable<FlightSeatClass> SeatClasses { get; set; } = new List<FlightSeatClass>();
        public string CalculateDuration(DateTime departure, DateTime arrival)
        {
            TimeSpan duration = arrival - departure;
            if (duration < TimeSpan.Zero)
            {
                throw new ArgumentException("Arrival time must be after the departure time.");
            }
            return $"{(int)duration.TotalHours}h {duration.Minutes}m";
        }
    }


}

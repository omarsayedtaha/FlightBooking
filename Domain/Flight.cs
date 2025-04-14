using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Flight 
    {
        public Guid Id { get; set; }

        public string FlightNumber { get; set; }
        public string Airline { get; set; }
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int AvailableSeats { get; set; }
        public decimal Price { get; set; }

        [NotMapped]
        public TimeOnly Duration { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

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

namespace Domain.Entities

{
    public class Booking
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Status { get; set; }

        public DateTime TravelDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public IEnumerable<FlightBookings> FlightBookings { get; set; } = new List<FlightBookings>();

        public Payments? Payments { get; set; }
    }

}
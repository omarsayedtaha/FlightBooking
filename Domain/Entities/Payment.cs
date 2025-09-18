namespace Domain.Entities
{
    public class Payments
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string StripeSessionId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public Booking Booking { get; set; }
    }
}


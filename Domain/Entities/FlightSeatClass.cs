using Domain.Entities;
using Domian.Enums;

namespace Domain.Entities
{
    public class FlightSeatClass
    {
        public int Id { get; set; }
        public SeatClass Class { get; set; }

        public decimal Price { get; set; }

        public int FlightId { get; set; }

        public Flight Flight { get; set; }
    }
}

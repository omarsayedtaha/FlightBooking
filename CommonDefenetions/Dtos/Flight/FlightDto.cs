using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDefenitions.Dtos.Flight
{
    public class FlightDto
    {
        public Guid Id { get; set; }
        public string FlightNumber { get; set; }
        public string Airline { get; set; }
        public string DepartureCity { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalLocation { get; set; }
        public string ArrivalTime { get; set; }
        public string Duration { get; set; }
        public decimal Price { get; set; }
    }
}

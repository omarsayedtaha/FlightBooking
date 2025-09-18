using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.Seats.SeatDto;

namespace Application.Dtos.Flight
{
    public class FlightDto
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public string Airline { get; set; }
        public string Class { get; set; }
        public string DepartureLocation { get; set; }
        public string DepartureDate { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalLocation { get; set; }
        public string ArrivalDate { get; set; }
        public string ArrivalTime { get; set; }
        public string Duration { get; set; }
        public decimal Price { get; set; }

        public List<SeatDto> Seats { get; set; }
    }
}

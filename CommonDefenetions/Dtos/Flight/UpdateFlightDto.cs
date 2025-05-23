﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDefenitions.Dtos.Flight
{
    public class UpdateFlightDto
    {
        public Guid FlightId { get; set; }
        public string FlightNumber { get; set; }
        public string Airline { get; set; }
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int NumberOfSeats { get; set; }
        public decimal Price { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Infrastructure;

namespace CommonDefenitions.Helper
{
    public static class DataSeeding
    {
        public static async void SeedData(ApplicationDbContext context)
        {
            if (!context.Flights.Any())
            {

                var flights = new List<Flight>
                {
                    new Flight
                    {
                        Airline = "SkyWay Airlines",
                        DepartureLocation = "New York, JFK",
                        ArrivalLocation = "London, LHR",
                        DepartureTime = new TimeOnly(8, 45), 
                        ArrivalTime = new TimeOnly(21, 30), 
                        AvailableSeats = 120,
                        PricePerSeat = 349m,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Flight
                    {
                        Airline = "SkyWay Airlines",
                        DepartureLocation = "New York, JFK",
                        ArrivalLocation = "London, LHR",
                        DepartureTime = new TimeOnly(11, 30), 
                        ArrivalTime = new TimeOnly(23, 45), 
                        AvailableSeats = 95,
                        PricePerSeat = 429m,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Flight
                    {
                        Airline = "SkyWay Airlines",
                        DepartureLocation = "New York, JFK",
                        ArrivalLocation = "London, LHR",
                        DepartureTime = new TimeOnly(18, 15), 
                        ArrivalTime = new TimeOnly(6, 30), 
                        AvailableSeats = 150,
                        PricePerSeat = 289m,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }

                };
                context.Flights.AddRange(flights);

            }
            await context.SaveChangesAsync();


        }
    }
}
